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
    public class RouteRepository : GenericRepository<Route>, IRouteRepository
    {
        private readonly FtravelContext _context;

        public RouteRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pagination<Route>> GetListRoutesAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Routes.CountAsync();
            var items = await _context.Routes.Include(x => x.BusCompany)
                                            .Include(x => x.StartPointNavigation)
                                            .Include(x => x.EndPointNavigation)
                                            .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                            .Take(paginationParameter.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();
            var result = new Pagination<Route>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }

        public async Task<Route?> GetRouteDetailByRouteIdAsync(int routeId)
        {
            return await _context.Routes
                .Include(x => x.BusCompany)
                .Include(x => x.StartPointNavigation)
                .Include(x => x.EndPointNavigation)
                .FirstOrDefaultAsync(x => x.Id == routeId);
        }
    }
}