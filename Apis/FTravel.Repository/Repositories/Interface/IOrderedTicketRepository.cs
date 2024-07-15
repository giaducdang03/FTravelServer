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
    public interface IOrderedTicketRepository : IGenericRepository<Order>
    {
        public Task<List<Order>> GetOrderedTicketListByCustomerId(int customer);
        //public Task<Order> GetOrderedTicketDetailByOrderId(int orderId);

        public Task<List<OrderDetail>> GetHistoryOfTripsTakenByCustomerId(int customer);
        // public Task<Pagination<Route>> GetHistoryOfTripsTakenByCustomerId(PaginationParameter paginationParameter, int customer);

        public Task<Pagination<OrderDetail>> GetOrderedTicketsByCustomer(int customerId, PaginationParameter paginationParameter);

        public Task<Customer> GetCustomerByTicketBoughtId(int ticketId);

    }
}
