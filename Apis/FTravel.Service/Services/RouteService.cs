using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Mvc.Abstractions;
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
                var map = _mapper.Map<Route>(route);
                var createRoute = await _routeRepository.CreateRoute(map);
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
                findRouteUpdate = _mapper.Map<Route>(routeUpdate);
                findRouteUpdate.Id = id;
                findRouteUpdate.UnsignName = StringUtils.ConvertToUnSign(routeUpdate.Name);
            }

            var result = await _routeRepository.UpdateRoutesAsync(findRouteUpdate);
            return result;
        }
    }
}
