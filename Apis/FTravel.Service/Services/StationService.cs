using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        private readonly IBusCompanyRepository _busCompanyRepository;
        private readonly IMapper _mapper;

        public StationService(IStationRepository stationRepository, 
            IBusCompanyRepository busCompanyRepository,
            IMapper mapper)
        {
            _stationRepository = stationRepository;
            _busCompanyRepository = busCompanyRepository;
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

        public async Task<StationModel> CreateStationService(string stationName, int buscompanyId)
        {
            var buscompany = await _busCompanyRepository.GetByIdAsync(buscompanyId);
            if (buscompany == null) 
            {
                throw new Exception("Nhà xe không tồn tại.");
            }
            var stationUnsign = StringUtils.ConvertToUnSign(stationName);
            var stations = await _stationRepository.GetAllAsync();
            var existStation = stations.Where(x => x.BusCompanyId == buscompanyId && x.UnsignName.ToLower() == stationUnsign.ToLower());

            if (!existStation.Any())
            {
                var newStation = new Station
                {
                    Name = stationName,
                    UnsignName = stationUnsign,
                    BusCompanyId = buscompanyId,
                    Status = CommonStatus.ACTIVE.ToString()
                };
                var createStation = await _stationRepository.createStation(newStation);
                var result = _mapper.Map<StationModel>(createStation);
                return result;
            } 
            else
            {
                throw new Exception("Trạm đã tồn tại.");
            }
        }


        public async Task<Pagination<StationModel>> GetAllStationService(PaginationParameter paginationParameter)
        {
            var stations = await _stationRepository.GetAllStation(paginationParameter);
            if (!stations.Any())
            {
                return null;
            }

            var stationModels = _mapper.Map<List<StationModel>>(stations);

            return new Pagination<StationModel>(stationModels,
                stations.TotalCount,
                stations.CurrentPage,
                stations.PageSize);

        }

        public async Task<Station> GetStationServiceDetailById(int id)
        {
            var data = await _stationRepository.GetStationById(id);
            return data;
        }
    }
}