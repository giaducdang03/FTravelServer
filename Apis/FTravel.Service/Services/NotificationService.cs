using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(INotificationRepository notificationRepository,
            IUserRepository userRepository) 
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task<Notification> AddNotificationByUserId(int userId, Notification notificationModel)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null) 
            {
                notificationModel.UserId = userId;
                return await _notificationRepository.AddAsync(notificationModel);
            }
            return null;
        }

        public async Task<Notification> GetNotificationById(int id)
        {
            return await _notificationRepository.GetByIdAsync(id);
        }

        public async Task<Pagination<Notification>> GetNotificationsByEmail(string email, PaginationParameter paginationParameter)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null) 
            { 
                return await _notificationRepository.GetNotificationsPagingByUserIdAsync(user.Id, paginationParameter);
            }
            return null;
        }

        public async Task<bool> MarkAllUserNotificationIsReadAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) 
            {
                throw new Exception("Tài khoản không tồn tại.");
            }

            var notifications = await _notificationRepository.GetAllNotificationsByUserIdAsync(user.Id);
            if (!notifications.Any())
            {
                return false;
            }
            var unreadNotifications = notifications.Where(n => n.IsRead ==  false);
            List<Notification> updateNotification = new List<Notification>();

            // mark read notification
            foreach (var notification in unreadNotifications) 
            { 
                notification.IsRead = true;
                updateNotification.Add(notification);
            }

            var result = await _notificationRepository.UpdateRangeAsync(updateNotification);

            return true ? result > 0 : false;
        }

        public async Task<bool> MarkNotificationIsReadById(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification != null)
            {
                if (notification.IsRead)
                {
                    return false;
                }

                notification.IsRead = true;
                var result = await _notificationRepository.UpdateAsync(notification);
                return true ? result > 0 : false;
            }
            return false;
        }

        public async Task<bool> PushMessageFirebase(string title, string body, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null) 
            {
                var fcmToken = user.Fcmtoken;
                if (fcmToken != null)
                {
                    await FirebaseLibrary.SendMessageFireBase(title, body, fcmToken);
                    return true;
                }
                throw new Exception("Not found FCM token.");
            }
            throw new Exception("Account does not exist.");
        }
    }
}
