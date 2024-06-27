using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.Commons.Filter;
using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.RouteModels;
using FTravel.Service.BusinessModels.StationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services.Interface
{
    public interface IStationService
    {
        public Task<Pagination<StationModel>> GetAllStationService(PaginationParameter paginationParameter);
        
        public Task<Station> GetStationServiceDetailById(int id);

        public Task<RouteModel> CreateRoute(RouteModel route);

        public Task<StationModel> CreateStationService(string stationName, int buscompanyId);
        public Task<int> UpdateStationService(UpdateStationModel updateStation, int stationId);
        public Task<bool> DeleteStationService(int stationId);
        public Task<List<StationModel>> GetStationByBusCompanyId(int id);
    }
}
