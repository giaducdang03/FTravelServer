﻿using FTravel.Repositories.Commons;
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
    public class TripRepository : GenericRepository<Trip>,  ITripRepository
    {
        private readonly FtravelContext _context;
        public TripRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Pagination<Trip>> GetAll(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Trips.CountAsync();
            var items = await _context.Trips
                                    .Include(x=> x.Route)
                                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .ToListAsync();
            var result = new Pagination<Trip>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }

        public async Task<Trip> GetTripById(int id)
        {
            return await _context.Trips
                .Include(x => x.Tickets)
                .Include(x => x.Route)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}