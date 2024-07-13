using FTravel.Repository.Commons.Filter;
using FTravel.Repository.Commons;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FTravel.Repositories.Commons;

namespace FTravel.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly FtravelContext _context;

        public OrderRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Transactions)
                .Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Pagination<OrderDetail>> GetAllOrderAsync(PaginationParameter paginationParameter, OrderFilter orderFilter)
        {
            var itemCount = await _context.Routes.CountAsync();
            var items = new List<OrderDetail>();
            if (orderFilter.BusCompanyName != null)
            {
                items = await _context.OrderDetails.Include(x => x.Ticket.Trip.Route.BusCompany).Where(x => x.Ticket.Trip.Route.BusCompany.UnsignName.ToLower().Contains(orderFilter.BusCompanyName.ToLower()))
                                                .Include(x => x.Order)
                                                .Include(x => x.Order.Customer)
                                                .Include(x => x.Ticket.Trip.Route)
                                                .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                                .Take(paginationParameter.PageSize)
                                                .AsNoTracking()
                                                .ToListAsync();
            }
            else
            {
                items = await _context.OrderDetails.Include(x => x.Ticket.Trip.Route.BusCompany)
                                                .Include(x => x.Order)
                                                .Include(x => x.Order.Customer)
                                                .Include(x => x.Ticket.Trip.Route)
                                                .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                                .Take(paginationParameter.PageSize)
                                                .AsNoTracking()
                                                .ToListAsync();
            }
            if (!string.IsNullOrWhiteSpace(orderFilter.SortBy))
            {
                switch (orderFilter.SortBy.ToLower())
                {
                    case "createdate":
                        items = orderFilter.Dir?.ToLower() == "asc" ? items.OrderBy(s => s.CreateDate).ToList() : items.OrderByDescending(s => s.CreateDate).ToList();
                        break;
                    case "tripname":
                        items = orderFilter.Dir?.ToLower() == "desc" ? items.OrderByDescending(s => s.Ticket.Trip.Name).ToList() : items.OrderBy(s => s.Ticket.Trip.Name).ToList();
                        break;
                    case "totalprice":
                        items = orderFilter.Dir?.ToLower() == "desc" ? items.OrderByDescending(s => s.Order.TotalPrice).ToList() : items.OrderBy(s => s.Order.TotalPrice).ToList();
                        break;
                    case "buscompanyname":
                        items = orderFilter.Dir?.ToLower() == "desc" ? items.OrderByDescending(s => s.Ticket.Trip.Route.BusCompany.UnsignName).ToList() : items.OrderBy(s => s.Ticket.Trip.Route.BusCompany.UnsignName).ToList();
                        break;
                    default:
                        items = items.OrderBy(s => s.Id).ToList(); // Default sort by Id
                        break;
                }
            }

            var result = new Pagination<OrderDetail>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }
        public async Task<List<OrderDetail>> GetOrderDetailByIdAsync(int id)
        {
            var result = await _context.OrderDetails.Include(x => x.Ticket)
                                            .ThenInclude(x => x.ServiceTickets)
                                            .ThenInclude(x => x.Service)
                                            .Include(x => x.Ticket.TicketType)
                                            .Include(x => x.Ticket.Trip)
                                            .Include(x => x.Ticket.Trip.Route)
                                            .Include(x => x.Ticket.Trip.Route.BusCompany)
                                            .Include(x => x.Ticket.Trip.Route.StartPointNavigation)
                                            .Include(x => x.Ticket.Trip.Route.EndPointNavigation)
                                            .Include(x => x.Order)
                                            .Include(x => x.Order.Transactions)
                                            .ThenInclude(x => x.Wallet)
                                            .Include(x => x.Order.Customer)
                                            .Where(x => x.OrderId == id)
                                            .ToListAsync();
            return result;
                                            
        }
        public async Task<List<OrderDetail>> StatisticForDashBoard()
        {
            var result = await _context.OrderDetails.Include(x => x.Ticket)
                                            .Include(x => x.Ticket.Trip)
                                            .Include(x => x.Ticket.Trip.Route)
                                            .Include(x => x.Ticket.Trip.Route.BusCompany)
                                            .Include(x => x.Ticket.Trip.Route.StartPointNavigation)
                                            .Include(x => x.Ticket.Trip.Route.EndPointNavigation)
                                            .Include(x => x.Order)
                                            .Include(x => x.Order.Customer)
                                            .ToListAsync();
            return result;
        }

    }
}
