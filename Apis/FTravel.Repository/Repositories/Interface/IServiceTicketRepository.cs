using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface IServiceTicketRepository : IGenericRepository<ServiceTicket>
    {
        public Task<List<ServiceTicket>> GetServiceTicketByTicketId(int ticketId);
    }
}
