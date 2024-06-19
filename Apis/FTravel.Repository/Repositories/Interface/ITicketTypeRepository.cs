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
    public interface ITicketTypeRepository : IGenericRepository<TicketType>
    {
        public Task<Pagination<TicketType>> GetAllTicketType(PaginationParameter paginationParameter);

        public Task<TicketType> GetTicketTypeByIdAsync(int id);

        public Task<TicketType> CreateTicketTypeAsync(TicketType ticketType);

    }
}
