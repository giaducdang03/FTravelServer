using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly FtravelContext _context;

        public NotificationRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetAllNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Pagination<Notification>> GetNotificationsPagingByUserIdAsync(int userId, PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Notifications.CountAsync();
            var items = await _context.Notifications.Where(x => x.UserId == userId)
                                    .OrderByDescending(x => x.CreateDate).Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Notification>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }
    }
}
