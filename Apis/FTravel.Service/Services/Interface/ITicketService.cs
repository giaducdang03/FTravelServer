using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.OrderModels;
using FTravel.Service.BusinessModels.TicketModels;

namespace FTravel.Service.Services.Interface
{
    public interface ITicketService
    {
        
        public Task<TicketModel> GetTicketByIdAsync(int id);

        //public Task<OrderModel> BuyTicketAsync(BuyTicketModel model);


        
    }
}
