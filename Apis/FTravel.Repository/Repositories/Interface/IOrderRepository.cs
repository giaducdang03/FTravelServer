using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<Order> GetOrderByIdAsync(int id);
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task<Pagination<OrderDetail>> GetAllOrderAsync(PaginationParameter paginationParameter, OrderFilter orderFilter);
        public Task<List<OrderDetail>> GetOrderDetailByIdAsync(int id);
        public Task<List<OrderDetail>> StatisticForDashBoard();

    }
}
