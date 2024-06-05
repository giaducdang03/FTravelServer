using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }


        public async Task<Pagination<Trip>> GetAllTripsWithStatusOpening(PaginationParameter paginationParameter)
        {
            var trips = await _tripRepository.ToPagination(paginationParameter);

            var filteredTrips = new List<Trip>();

            foreach (var trip in trips)
            {
                if (trip.Status == "OPENING")
                {
                    filteredTrips.Add(trip);
                }
            }

            if (filteredTrips.Count > 0)
            {
                return new Pagination<Trip>(filteredTrips,
                    trips.TotalCount,
                    trips.CurrentPage,
                    trips.PageSize);
            }
            else
            {
                throw new Exception("No trips are opening at the moment");
            }
        }

        public async Task<Trip> GetTripDetailByIdWithStatusOpening(int id)
        {
            var trip = await _tripRepository.GetTripDetailById(id);
            if (trip != null && trip.Status == "OPENING")
            {
                return trip;
            }
            else
            {
                throw new Exception("Trip is not opening at the moment");
            }
        }
    }
}
