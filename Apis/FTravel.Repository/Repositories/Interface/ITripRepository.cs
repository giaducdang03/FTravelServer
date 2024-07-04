using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Repository.Repositories.Interface
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        public Task<Trip> GetTripById(int id);
        Task<bool> UpdateTripAsync(Trip trip);
        public Task<Pagination<Trip>> GetAllTrips(PaginationParameter paginationParameter, TripFilter filter);
        public Task<Trip> GetTemplateTrip();
        public Task<bool> HasOverlappingTrip(int driverId, DateTime? newTripStart, DateTime? newTripEnd);
        public Task<List<TripService>> GetServiceByTripId(int id);
    }
}
