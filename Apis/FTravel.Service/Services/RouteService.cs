using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels.RouteModels;
using FTravel.Service.BusinessModels.ServiceModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IMapper _mapper;

        public RouteService(IRouteRepository routeRepository, IMapper mapper)
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
        }
        public async Task<Pagination<RouteModel>> GetListRouteAsync(PaginationParameter paginationParameter)
        {
            var routes = await _routeRepository.GetListRoutesAsync(paginationParameter);
            if(!routes.Any())
            {
                return null;
            }

            var routeModels = routes.Select(x => new RouteModel
            {
                Id = x.Id,
                Name = x.Name,
                UnsignName = x.UnsignName,
                CreateDate = x.CreateDate,
                UpdateDate = x.UpdateDate,
                StartPoint = x.StartPointNavigation.Name,
                EndPoint = x.EndPointNavigation.Name,
                Status = x.Status,
                BusCompanyName = x.BusCompany.Name,
                IsDeleted = x.IsDeleted,
            }).ToList();
            return new Pagination<RouteModel>(routeModels, 
                routes.TotalCount, 
                routes.CurrentPage, 
                routes.PageSize);
        }
        

        public async Task<CreateRouteModel> CreateRoute(CreateRouteModel route)
        {
            try
            {
                var startPoint = route.StartPoint;
                var endPoint = route.EndPoint;
                //check
                var routeExists = await _routeRepository.CheckRouteExists(startPoint, endPoint);
                if (routeExists)
                {
                    throw new Exception("Tuyến đường đã tồn tại.");
                }

                var map = _mapper.Map<Route>(route);
                map.Status = CommonStatus.ACTIVE.ToString();
                map.UnsignName = StringUtils.ConvertToUnSign(map.Name);
                var createRoute = await _routeRepository.AddAsync(map);
                var resutl = _mapper.Map<CreateRouteModel>(createRoute);
                return resutl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<RouteModel?> GetRouteDetailByRouteIdAsync(int routeId)
        {
            var route = await _routeRepository.GetRouteDetailByRouteIdAsync(routeId);
            if(route == null)
            {
                return null;
            }

            var routeModel = _mapper.Map<RouteModel>(route);
            routeModel.StartPoint = route.StartPointNavigation.Name;
            routeModel.EndPoint = route.EndPointNavigation.Name;
            routeModel.BusCompanyName = route.BusCompany.Name;
            routeModel.RouteStations = _mapper.Map<List<RouteStationModel>>(route.RouteStations);
            routeModel.Services = _mapper.Map<List<ServiceModel>>(route.Services);
            return routeModel;

        }

        public async Task<int> RouteSoftDeleteAsync(int routeId)
        {
            var routeSoftDelete = await _routeRepository.SoftDeleteRoute(routeId);
            return routeSoftDelete;
            
        }

        public async Task<int> UpdateRouteAsync(UpdateRouteModel routeUpdate, int id)
        {
            var findRouteUpdate = await _routeRepository.GetRouteDetailByRouteIdAsync(id);
            if(findRouteUpdate == null)
            {
                return -1;
            } else
            {
                findRouteUpdate.Name = routeUpdate.Name;
                findRouteUpdate.StartPoint = routeUpdate.StartPoint;
                findRouteUpdate.EndPoint = routeUpdate.EndPoint;
                findRouteUpdate.Status = routeUpdate.Status.ToString();
                findRouteUpdate.BusCompanyId = routeUpdate.BusCompanyId;
                findRouteUpdate.UnsignName = StringUtils.ConvertToUnSign(routeUpdate.Name);
            }

            var result = await _routeRepository.UpdateAsync(findRouteUpdate);
            return result;
        }

        public Task<int> AddStationForRoute(AddStationForRouteModel addStation)
        {
            var addRouteStation = new RouteStation()
            {
                RouteId = addStation.RouteId,
                StationId = addStation.StationId,
                StationIndex = addStation.StationIndex,
                CreateDate = DateTime.UtcNow.AddHours(7),
                IsDeleted = false
            };
            var result = _routeRepository.AddStationForRoute(addRouteStation);
            return result;
        }

        public async Task<bool> ChangeStationIndex(IEnumerable<ChangeStationModel> changeStation)
        {
            if (changeStation.Count() == 2 && changeStation.First().RouteId == changeStation.Last().RouteId)
            {
                var listRouteStation = new List<RouteStation>();
                var routeStationFirst = new RouteStation()
                {
                    RouteId = changeStation.First().RouteId,
                    StationId = changeStation.First().StationId,
                    StationIndex = changeStation.First().StationIndex,
                };
                listRouteStation.Add(routeStationFirst);
                var routeStationLast = new RouteStation()
                {
                    RouteId = changeStation.Last().RouteId,
                    StationId = changeStation.Last().StationId,
                    StationIndex = changeStation.Last().StationIndex
                };
                listRouteStation.Add(routeStationLast);
                var result = await _routeRepository.ChangeStationIndex(listRouteStation);
                return result;
            }
            return false;
        }


    }
}
