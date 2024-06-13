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
    public class OrderedTicketRepository : GenericRepository<Order>, IOrderedTicketRepository
    {
        private readonly FtravelContext _context;
        public OrderedTicketRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Order> GetOrderedTicketDetailByOrderId(int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId)
                .Select(o => new Order
                {
                    Code = o.Code,
                    TotalPrice = o.TotalPrice,
                    PaymentDate = o.PaymentDate,
                    PaymentStatus = o.PaymentStatus,
                    CustomerId = o.CustomerId
                })
                .FirstOrDefaultAsync();

            if (order != null)
            {
                var orderDetails = await _context.OrderDetails
                    .Where(od => od.OrderId == orderId)
                    .Select(od => new OrderDetail
                    {
                        TicketId = od.TicketId,
                        OrderId = od.OrderId,
                        Type = od.Type,
                        ServiceName = od.ServiceName,
                        UnitPrice = od.UnitPrice,
                        Quantity = od.Quantity,
                        Ticket = new Ticket()
                    })
                    .ToListAsync();

                foreach (var orderDetail in orderDetails)
                {
                    var ticket = await _context.Tickets.FindAsync(orderDetail.TicketId);
                    if (ticket != null)
                    {
                        orderDetail.Ticket.TripId = ticket.TripId;
                        orderDetail.Ticket.TicketTypeId = ticket.TicketTypeId;
                        orderDetail.Ticket.SeatCode = ticket.SeatCode;
                        orderDetail.Ticket.Status = ticket.Status;

                        var ticketType = await _context.TicketTypes
                            .Where(tt => tt.Id == ticket.TicketTypeId)
                            .Select(tt => new TicketType
                            {
                                Name = tt.Name,
                                RouteId = tt.RouteId,
                                Price = tt.Price
                            })
                            .FirstOrDefaultAsync();

                        if (ticketType != null)
                        {
                            orderDetail.Ticket.TicketType = ticketType;
                        }
                    }
                }

                order.OrderDetails = orderDetails;
            }

            return order;
        }


        public async Task<List<Order>> GetOrderedTicketListByCustomerId(int customer)
        {
            var data = await _context.Orders.Where(x => x.CustomerId.Equals(customer)).ToListAsync();   
            return data;
        }
    }
}
