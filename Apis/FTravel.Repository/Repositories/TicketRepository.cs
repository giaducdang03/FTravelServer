using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly FtravelContext _context;

        public TicketRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<List<Ticket>> GetAll()
        {
            return await _context.Tickets
                                .Include(x => x.TicketType)
                                .ToListAsync();
        }

        public async Task<List<Ticket>> GetAllByTripId(int tripId)
        {
            return await _context.Tickets
                                .Include(x => x.TicketType)
                                .Where(x => x.TripId == tripId)
                                .ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.Include(x => x.TicketType).FirstOrDefaultAsync(x => x.Id == ticketId);
        }

        public async Task<Ticket> GetTripDetailById(int id)
        {
            return await _context.Tickets
                                .Include(x => x.TicketType)
                                .FirstOrDefaultAsync(x => x.Id == id);
        }

        
    }
}
