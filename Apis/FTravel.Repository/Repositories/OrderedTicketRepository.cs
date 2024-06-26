﻿using FTravel.Repository.DBContext;
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
    }
}
