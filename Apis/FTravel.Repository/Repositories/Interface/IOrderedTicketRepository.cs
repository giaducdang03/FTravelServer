using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IOrderedTicketRepository : IGenericRepository<Order>
    {
        public Task<List<Order>> GetOrderedTicketListByCustomerId(int customer);
        public Task<Order> GetOrderedTicketDetailByOrderId(int orderId);
    }
}
