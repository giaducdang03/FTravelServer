using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        public Task<Pagination<Notification>> GetNotificationsPagingByUserIdAsync(int userId, PaginationParameter paginationParameter);

        public Task<List<Notification>> GetAllNotificationsByUserIdAsync(int userId); 

    }
}
