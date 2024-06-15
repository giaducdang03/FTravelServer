using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IUserService _userService;

        public NotificationService(INotificationRepository notificationRepository,
            IUserService userService)
        {
            _notificationRepository = notificationRepository;
            _userService = userService;
        }

        public async Task<Notification> AddNotificationByListUser(List<int> userIds, Notification notificationModel)
        {
            var users = await _userService.GetUsersByUserIdsAsync(userIds);
            if (users.Any())
            {
                List<Notification> notificationList = new List<Notification>();
                foreach (int userId in userIds)
                {
                    var newNoti = new Notification
                    {
                        UserId = userId,
                        Type = NotificationType.SYSTEM.ToString(),
                        Title = notificationModel.Title,
                        Message = notificationModel.Message,
                    };
                    notificationList.Add(newNoti);
                }
                await _notificationRepository.AddRangeAsync(notificationList);

                // push notification
                var userTokens = users.Where(x => !x.Fcmtoken.IsNullOrEmpty()).Select(x => x.Fcmtoken).ToList();
                await PushListMessageFirebase(notificationModel.Title, notificationModel.Message, userTokens);

                return notificationModel;
            }
            return null;
        }

        public async Task<Notification> AddNotificationByRoleAsync(RoleEnums roleEnums, Notification notificationModel)
        {
            var users = await _userService.GetUsersByRoleAsync(roleEnums);
            if (users.Any())
            {
                List<int> userIds = users.Select(x => x.Id).ToList();
                List<Notification> notificationList = new List<Notification>();
                foreach (int userId in userIds)
                {
                    var newNoti = new Notification
                    {
                        UserId = userId,
                        Type = NotificationType.SYSTEM.ToString(),
                        Title = notificationModel.Title,
                        Message = notificationModel.Message,
                    };
                    notificationList.Add(newNoti);
                }
                await _notificationRepository.AddRangeAsync(notificationList);

                // push notification
                var userTokens = users.Where(x => !x.Fcmtoken.IsNullOrEmpty()).Select(x => x.Fcmtoken).ToList();
                await PushListMessageFirebase(notificationModel.Title, notificationModel.Message, userTokens);

                return notificationModel;
            }
            return null;
        }

        public async Task<Notification> AddNotificationByUserId(int userId, Notification notificationModel)
        {
            var user = await _userService.GetUserByIdAsync(userId);
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
            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                return await _notificationRepository.GetNotificationsPagingByUserIdAsync(user.Id, paginationParameter);
            }
            return null;
        }

        public async Task<bool> MarkAllUserNotificationIsReadAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Tài khoản không tồn tại.");
            }

            var notifications = await _notificationRepository.GetAllNotificationsByUserIdAsync(user.Id);
            if (!notifications.Any())
            {
                return false;
            }
            var unreadNotifications = notifications.Where(n => n.IsRead == false);
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
            var user = await _userService.GetUserByIdAsync(userId);
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

        public async Task<bool> PushListMessageFirebase(string title, string body, List<string> fcmTokens)
        {
            if (fcmTokens.Any())
            {
                await FirebaseLibrary.SendRangeMessageFireBase(title, body, fcmTokens);
                return true;
            }
            throw new Exception("Not found fcm token.");
        }
    }
}
