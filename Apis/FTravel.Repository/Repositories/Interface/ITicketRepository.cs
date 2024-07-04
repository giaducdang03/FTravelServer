using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        public Task<List<Ticket>> GetAll();
        public Task<List<Ticket>> GetAllByTripId(int tripId);
        public Task<Ticket> GetTripDetailById(int id);
        public Task<Ticket> CreateTicket(Ticket ticket);
        
        public Task<Ticket> GetTicketByIdAsync(int ticketId);
    }
}
