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
    public class RouteRepository : GenericRepository<Route>, IRouteRepository
    {
        private readonly FtravelContext _context;

        public RouteRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Route> CreateRoute(Route route)
        {
            _context.Add(route);
            await _context.SaveChangesAsync();
            return route;
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
                .Include(x => x.Services)
                .Include(x=> x.TicketTypes)
                .FirstOrDefaultAsync(x => x.Id == routeId);
        }

        public async Task<List<Route>> GetRoutesByIdsAsync(IEnumerable<int> routeId)
        {
            return await _context.Routes
                                 .Where(c => routeId.Contains(c.Id))
                                 .ToListAsync();
        }

        public async Task<int> UpdateRoutesAsync(Route route)
        {
            try
            {
                var updateRoute = await _context.Routes.FirstOrDefaultAsync(x => x.Id == route.Id);
                if(updateRoute == null)
                {
                    return -1;
                }
                updateRoute.Name = route.Name;
                updateRoute.StartPoint = route.StartPoint;
                updateRoute.EndPoint = route.EndPoint;
                updateRoute.Status = route.Status;
                updateRoute.BusCompanyId = route.BusCompanyId;
                updateRoute.UpdateDate = DateTime.UtcNow.AddHours(7);
                updateRoute.UnsignName = route.UnsignName;
                _context.Routes.Update(updateRoute);
                var result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<int> SoftDeleteRoute(int routeId)
        {
            try
            {
                var routeSoftDelete = await _context.Routes.FirstOrDefaultAsync(x => x.Id == routeId);
                if (routeSoftDelete != null)
                {
                    routeSoftDelete.IsDeleted = true;
                    var result = await  _context.SaveChangesAsync();
                    return result;
                }
                return -1;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

    }
}
