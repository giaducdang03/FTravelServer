using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
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
        public async Task<Pagination<Service>> GetAll(PaginationParameter paginationParameter, ServiceFilter filter)
        {
            var query = _context.Services.Where(x => x.IsDeleted == false)
                                 .Include(x => x.Route)
                                 .Include(x => x.Station)
                                 .AsQueryable();
            // Apply filtering
            query = ApplyFiltering(query, filter);

            var itemCount = await query.CountAsync();
            var items = await query
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
            var items = await _context.Services.Where(s => s.RouteId == routeId && s.IsDeleted == false)
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
            var items =  await _context.Services.Where(s => s.StationId == stationId && s.IsDeleted == false)
                                    .Include(x => x.Route)
                                    .Include(x => x.Station)
                                    .OrderByDescending(s => s.DefaultPrice)
                                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .ToListAsync();
            var result = new Pagination<Service>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            return result;
        }
        public IQueryable<Service> ApplyFiltering(IQueryable<Service> query, ServiceFilter filter)
        {   
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(s => s.Route.UnsignName.Contains(filter.Search) ||
                                         s.Station.UnsignName.Contains(filter.Search) ||
                                         s.UnsignName.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                switch (filter.SortBy.ToLower())
                {
                    case "routename":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.Route.UnsignName) : query.OrderBy(s => s.Route.UnsignName);
                        break;
                    case "stationname":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.Station.UnsignName) : query.OrderBy(s => s.Station.UnsignName);
                        break;
                    case "name":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.UnsignName) : query.OrderBy(s => s.UnsignName);
                        break;
                    case "defaultprice":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(s => s.DefaultPrice) : query.OrderBy(s => s.DefaultPrice);
                        break;
                    default:
                        query = query.OrderBy(s => s.Id); // Default sort by Id
                        break;
                }
            }

            return query;
        }
    }
}
