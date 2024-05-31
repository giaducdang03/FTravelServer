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
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly FtravelContext _context;
        public TicketRepository(FtravelContext context) : base(context)
        {
            _context = context;
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
    }
}
