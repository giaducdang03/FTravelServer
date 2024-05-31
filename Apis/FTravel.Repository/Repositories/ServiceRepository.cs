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
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly FtravelContext _context;
        public ServiceRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Pagination<Service>> GetAll(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Services.CountAsync();
            var items = await _context.Services
                                    .Include(x => x.Route)
                                    .Include(x => x.Station)
                                    .OrderByDescending(s => s.DefaultPrice)
                                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .ToListAsync();
            var result = new Pagination<Service>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }
        public async Task<Service> GetServiceById(int id)
        {
            return await _context.Services
                .Include(x => x.Route)
                .Include(x => x.Station)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Pagination<Service>> GetAllServiceByRouteId(int routeId, PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Services.CountAsync();
            var items = await _context.Services.Where(s => s.RouteId == routeId)
                                    .Include(x => x.Route)
                                    .Include(x=> x.Station)
                                    .OrderByDescending(s => s.DefaultPrice)
                                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .ToListAsync();
            var result = new Pagination<Service>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }
        public async Task<Pagination<Service>> GetAllServiceByStationId(int stationId, PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Services.CountAsync();
            var items =  await _context.Services.Where(s => s.StationId == stationId)
                                    .Include(x => x.Route)
                                    .Include(x => x.Station)
                                    .OrderByDescending(s => s.DefaultPrice)
                                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .ToListAsync();
            var result = new Pagination<Service>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }
    }
}
