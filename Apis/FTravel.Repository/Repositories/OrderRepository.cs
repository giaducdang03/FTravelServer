﻿using FTravel.Repository.DBContext;
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
        public async Task<List<Order>> GetAllOrderAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer).ToListAsync();
        }
        public async Task<List<OrderDetail>> GetOrderDetailByIdAsync(int id)
        {
            var result = await _context.OrderDetails.Include(x => x.Ticket)
                                            .Include(x => x.Ticket.Trip)
                                            .Include(x => x.Ticket.Trip.Route)
                                            .Include(x => x.Ticket.Trip.Route.BusCompany)
                                            .Include(x => x.Ticket.Trip.Route.StartPointNavigation)
                                            .Include(x => x.Ticket.Trip.Route.EndPointNavigation)
                                            .Include(x => x.Order)
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
