using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories
{
    public class OrderedTicketRepository : GenericRepository<Order>, IOrderedTicketRepository
    {
        private readonly FtravelContext _context;
        public OrderedTicketRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OrderDetail>> GetHistoryOfTripsTakenByCustomerId(int customer)
        {
            var data = await _context.OrderDetails

        .Include(o => o.Order)
        .Include(o => o.Ticket.Trip)
        .ThenInclude(t => t.Route)
        .ThenInclude(r => r.StartPointNavigation)

        .Include(o => o.Order)
        .Include(o => o.Ticket.Trip)
        .ThenInclude(t => t.Route)
        .ThenInclude(r => r.EndPointNavigation)

        .Include(o => o.Order)
        .Include(o => o.Ticket.Trip)
        .ThenInclude(t => t.Route)
        .ThenInclude(r => r.BusCompany)
        .Where(o => o.Order.CustomerId == customer)

        .ToListAsync();

            return data;
        }
        public async Task<List<Order>> GetOrderedTicketListByCustomerId(int customer)
        {
            var data = await _context.Orders.Where(x => x.CustomerId.Equals(customer)).ToListAsync();
            return data;
        }

        public async Task<Pagination<OrderDetail>> GetOrderedTicketsByCustomer(int customerId, PaginationParameter paginationParameter)
        {
            var orders = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Ticket.Trip)
                .ThenInclude(t => t.Route)
                .ThenInclude(r => r.StartPointNavigation)

                .Include(o => o.Order)
                .Include(o => o.Ticket.Trip)
                .ThenInclude(t => t.Route)
                .ThenInclude(r => r.EndPointNavigation)

                .Include(o => o.Order)
                .Include(o => o.Ticket.Trip)
                .ThenInclude(t => t.Route)
                .ThenInclude(r => r.BusCompany)
                .Where(o => o.Order.CustomerId == customerId).OrderByDescending(o => o.CreateDate)
                .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize)
                .AsNoTracking()
            .ToListAsync();

            var itemCount = await _context.OrderDetails.Where(o => o.Order.CustomerId == customerId).CountAsync();
            var result = new Pagination<OrderDetail>(orders, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }
    }
}
