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
        public Task<List<Order>> GetAllOrderAsync();
        public Task<List<OrderDetail>> GetOrderDetailByIdAsync(int id);
        public Task<List<OrderDetail>> StatisticForDashBoard();


    }
}