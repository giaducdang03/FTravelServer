using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IOrderedTicketService
    {
        public Task<List<Order>> GetAllOrderedTicketByCustomerIdService(int customer);
        //public Task<Order> GetAllOrderedTicketDetailByOrderIdService(int orderId);


    }
}
