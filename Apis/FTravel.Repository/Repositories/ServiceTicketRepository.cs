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
    public class ServiceTicketRepository : GenericRepository<ServiceTicket>, IServiceTicketRepository
    {
        private readonly FtravelContext _context;

        public ServiceTicketRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ServiceTicket>> GetServiceTicketByTicketId(int ticketId)
        {
            return await _context.ServiceTickets.Where(x => x.TicketId == ticketId).ToListAsync();
        }
    }
}
