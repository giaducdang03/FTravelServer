using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTravel.Service.BusinessModels;

namespace FTravel.Service.Services.Interface
{
    public interface ITicketService
    {
        
        public Task<TicketModel> GetTicketByIdAsync(int id);

        
    }
}
