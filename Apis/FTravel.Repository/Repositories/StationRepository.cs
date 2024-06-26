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
    public class StationRepository : GenericRepository<Station>, IStationRepository
    {
        private readonly FtravelContext _context;

        public StationRepository(FtravelContext context) : base(context)
        {
            {
                _context = context;
            }
        }

        public async Task<Route> CreateRoute(Route route)
        {
            _context.Add(route);
            await _context.SaveChangesAsync();  
            return route;   
        }

        public async Task<Station> createStation(Station station)
        {
            _context.Add(station);
            await _context.SaveChangesAsync();
            return station;
        }

        

        public async Task<Pagination<Station>> GetAllStation(PaginationParameter paginationParameter, StationFilter stationFilter)
        {
            var query = _context.Stations.Include(s => s.BusCompany).AsQueryable();

            var totalCount = await query.CountAsync();
            var paginatedQuery = query.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                      .Take(paginationParameter.PageSize);

            var stations = await paginatedQuery.ToListAsync();

            try
            {
                if (!string.IsNullOrEmpty(stationFilter.Search))
                {
                    stations = stations.Where(x => x.BusCompany.UnsignName.ToLower().Contains(stationFilter.Search.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(stationFilter.SortBy))
                {
                    if (stationFilter.SortBy.Equals("bus-company-id"))
                    {
                        if(stationFilter.Dir.ToLower().Equals("asc"))
                        {
                            stations = stations.OrderBy(x => x.BusCompanyId).ToList();
                        }
                        if(stationFilter.Dir.ToLower().Equals("desc"))
                        {
                            stations = stations.OrderByDescending(x => x.BusCompanyId).ToList();
                        }
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }

            return new Pagination<Station>(stations, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
        }

        public async Task<Station> GetStationById(int id)
        {
            var station = await _context.Stations.Include(s => s.BusCompany).FirstOrDefaultAsync( x => x.Id == id);
            return station;
        }
        public async Task<List<RouteStation>> GetRouteStationById(int id)
        {
            var station = await _context.RouteStations.Where(x => x.StationId == id).ToListAsync();
            return station;
        }
    }
}
