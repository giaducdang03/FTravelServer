using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Service.BusinessModels.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IServiceService
    {
        public Task<Pagination<ServiceModel>> GetAllAsync(PaginationParameter paginationParameter, ServiceFilter filter);
        public Task<ServiceModel> GetServiceByIdAsync(int id);
        public Task<Pagination<ServiceModel>> GetAllServiceByRouteIdAsync(int routeId, PaginationParameter paginationParameter);
        public Task<Pagination<ServiceModel>> GetAllServiceByStationIdAsync(int stationId, PaginationParameter paginationParameter);
        public Task<bool> AddServiceAsync(CreateServiceModel serviceToCreate);
        public Task<bool> UpdateServiceAsync(int id, UpdateServiceModel serviceToUpdate);
        public Task<bool> DeleteServiceAsync(int id);
    }
}
