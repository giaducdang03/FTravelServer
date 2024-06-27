using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.RouteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IRouteService
    {
        public Task<Pagination<RouteModel>> GetListRouteAsync(PaginationParameter paginationParameter);
        public Task<RouteModel?> GetRouteDetailByRouteIdAsync(int routeId);

        public Task<int> UpdateRouteAsync(UpdateRouteModel routeUpdate, int id);
        public Task<int> RouteSoftDeleteAsync(int routeId);

        public Task<CreateRouteModel> CreateRoute(CreateRouteModel route);

        public Task<int> AddStationForRoute(AddStationForRouteModel addStation);

    }
}
