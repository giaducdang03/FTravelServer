using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.DBContext;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FTravel.Repository.Repositories
{
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {
        private readonly FtravelContext _context;

        public TripRepository(FtravelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pagination<Trip>> GetAllTrips(PaginationParameter paginationParameter, TripFilter filter)
        {
            var query = _context.Trips.Where(x => x.IsDeleted == false)
                                      .Include(x => x.Route)
                                      .Include(x => x.Route.StartPointNavigation)
                                      .Include(x => x.Route.EndPointNavigation)
                                      .Include(r => r.Route.BusCompany)
                                      .AsQueryable();

            // Apply filtering
            query = ApplyTripFiltering(query, filter);

            var itemCount = await query.CountAsync();

            var items = await query
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
                .ThenInclude(r => r.BusCompany)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateTripAsync(Trip trip)
        {
            try
            {
                _context.Trips.Update(trip);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Trip> GetTemplateTrip()
        {
            return await _context.Trips
                 .Include(x => x.Tickets)
                 .ThenInclude(x => x.TicketType)
                 .Include(x => x.Route)
                 .Where(x => x.IsTemplate == true)
                 .FirstAsync();

        }

        public async Task<bool> HasOverlappingTrip(int driverId, DateTime? newTripStart, DateTime? newTripEnd)
        {
            if (newTripStart == null || newTripEnd == null)
            {
                return false;
            }
            var trips = await _context.Trips
            .Where(t => t.DriverId == driverId &&
                    t.EstimatedStartDate < newTripEnd &&
                    t.EstimatedEndDate > newTripStart)
            .ToListAsync();

            return trips.Any();
        }
        public IQueryable<Trip> ApplyTripFiltering(IQueryable<Trip> query, TripFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(t => t.Route.UnsignName.Contains(filter.Search) ||
                                         t.Route.BusCompany.UnsignName.Contains(filter.Search) ||
                                         t.UnsignName.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.TripStatus))
            {
                query = query.Where(t => t.Status == filter.TripStatus);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(t => t.EstimatedStartDate.Value.Date == filter.StartDate.Value.Date);
            }
            if (filter.StartPoint.HasValue)
            {
                query = query.Where(t => t.Route.StartPointNavigation.Id == filter.StartPoint);
            }

            if (filter.EndPoint.HasValue)
            {
                query = query.Where(t => t.Route.EndPointNavigation.Id == filter.EndPoint);
            }
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                switch (filter.SortBy.ToLower())
                {
                    case "routename":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(t => t.Route.UnsignName) : query.OrderBy(t => t.Route.UnsignName);
                        break;
                    case "buscompanyname":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(t => t.Route.BusCompany.UnsignName) : query.OrderBy(t => t.Route.BusCompany.UnsignName);
                        break;
                    case "startdate":
                        query = filter.Dir?.ToLower() == "desc" ? query.OrderByDescending(t => t.EstimatedStartDate) : query.OrderBy(t => t.EstimatedStartDate);
                        break;
                    default:
                        query = query.OrderBy(t => t.Id); // Default sort by Id
                        break;
                }
            }

            return query;
        }
        public async Task<List<TripService>> GetServiceByTripId(int tripId)
        {
            var tripServices = await _context.TripServices
                                     .Include(ts => ts.Service)
                                     .Where(ts => ts.TripId == tripId)
                                     .ToListAsync();
            return tripServices;
        }

        public async Task<bool> CheckServiceInTrip(int tripId, int serviceId)
        {
            var trip = _context.Trips.Include(x => x.TripServices)
                                                          .Where(x => x.Id == tripId)
                                                          .FirstOrDefault();
            foreach (var item in trip.TripServices)
            {
                if (item.ServiceId == serviceId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
