using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
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
    public class TicketTypeRepository : GenericRepository<TicketType>, ITicketTypeRepository
    {
        private readonly FtravelContext _context;

        public TicketTypeRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TicketType> CreateTicketTypeAsync(TicketType ticketType)
        {
            _context.Add(ticketType);
            await _context.SaveChangesAsync();
            return ticketType;
        }

        public async Task<Pagination<TicketType>> GetAllTicketType(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.TicketTypes.CountAsync();
            var items = await _context.TicketTypes.OrderBy(x => x.Name).Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<TicketType>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public Task<TicketType> GetTicketTypeByIdAsync(int id)
        {
            return _context.TicketTypes.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
