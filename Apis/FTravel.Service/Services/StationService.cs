using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public StationService(IStationRepository stationRepository, IMapper mapper)
        {
            _stationRepository = stationRepository;
            _mapper = mapper;
        }

        public async Task<RouteModel> CreateRoute(RouteModel route)
        {
            try
            {
                var map = _mapper.Map<Route>(route);
                var createRoute = await _stationRepository.CreateRoute(map);
                var resutl = _mapper.Map<RouteModel>(createRoute);
                return resutl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<StationModel> CreateStationService(StationModel station)
        {
            try
            {
                var paginationParameter = new PaginationParameter
                {
                    PageIndex = 1,
                    PageSize = 10
                };
                var data = await _stationRepository.GetAllStation(paginationParameter);
                var checkExist = data.Where(x => x.Name.Equals(station.Name));

                if (checkExist.Any())
                {
                    return null;
                }

                var map = _mapper.Map<Station>(station);
                var createStation = await _stationRepository.createStation(map);
                var result = _mapper.Map<StationModel>(createStation);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        public async Task<Pagination<StationModel>> GetAllStationService(PaginationParameter paginationParameter)
        {
            var routes = await _stationRepository.GetAllStation(paginationParameter);
            if (!routes.Any())
            {
                return null;
            }

            var stationModels = _mapper.Map<List<StationModel>>(routes);
            foreach (var stationModel in stationModels)
            {
                stationModel.Id = stationModel.Id; 
            }

            return new Pagination<StationModel>(stationModels,
                routes.TotalCount,
                routes.CurrentPage,
                routes.PageSize);

        }

        public async Task<Station> GetStationServiceDetailById(int id)
        {
            var data = await _stationRepository.GetStationById(id);
            return data;
        }
    }
}