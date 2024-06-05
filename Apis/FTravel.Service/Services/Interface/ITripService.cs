using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface ITripService
    {
        public Task<Pagination<TripModel>> GetAllAsync(PaginationParameter paginationParameter);
        public Task<TripModel> GetTripByIdAsync(int id);
        public Task<bool> CreateTripAsync(CreateTripModel trip);
        public Task<bool> UpdateTripAsync(UpdateTripModel tripModel);
        public Task<Pagination<Trip>> GetAllTripsWithStatusOpening(PaginationParameter paginationParameter);

        public Task<Trip> GetTripDetailByIdWithStatusOpening(int id);
    }
}
