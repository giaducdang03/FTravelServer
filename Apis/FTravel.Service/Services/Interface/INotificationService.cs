using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface INotificationService
    {
        public Task<Notification> AddNotificationByUserId(int userId, Notification notificationModel);

        public Task<Notification> AddNotificationByCustomerId(int customerId, Notification notificationModel);

        public Task<Notification> AddNotificationByRoleAsync(RoleEnums roleEnums, Notification notificationModel);

        public Task<Notification> AddNotificationByListUser(List<int> userIds, Notification notificationModel);

        public Task<Notification> GetNotificationById(int id);

        public Task<Pagination<Notification>> GetNotificationsByEmail(string email, PaginationParameter paginationParameter);

        public Task<bool> PushMessageFirebase(string title, string body, int userId);

        public Task<bool> PushMessagePaymentFirebase(string title, string body, int userId);

        public Task<bool> MarkAllUserNotificationIsReadAsync(string email);

        public Task<bool> MarkNotificationIsReadById(int notificationId);

        public Task<bool> PushListMessageFirebase(string title, string body, List<string> fcmTokens);
    }
}
