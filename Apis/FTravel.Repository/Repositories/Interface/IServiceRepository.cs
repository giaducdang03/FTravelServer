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
    public interface IServiceRepository : IGenericRepository<Service>
    {
        public Task<Pagination<Service>> GetAll(PaginationParameter paginationParameter, ServiceFilter filter);
        public Task<Service> GetServiceById(int id);
        public Task<Pagination<Service>> GetAllServiceByRouteId(int routeId, PaginationParameter paginationParameter);
        public Task<Pagination<Service>> GetAllServiceByStationId(int stationId, PaginationParameter paginationParameter);
    }
}
