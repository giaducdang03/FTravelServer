using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.TicketModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FTravel.Service.Services
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly ITicketTypeRepository _ticketTypeRepository;
        private readonly IMapper _mapper;
        private readonly IRouteRepository _routeRepository;

        public TicketTypeService(ITicketTypeRepository ticketTypeRepository, IRouteRepository routeRepository, IMapper mapper)
        {
            _ticketTypeRepository = ticketTypeRepository;
            _routeRepository = routeRepository;
            _mapper = mapper;
        }

        public async Task<CreateTicketTypeModel> CreateTicketTypeAsync(CreateTicketTypeModel ticketTypeModel)
        {

            try
            {
                var route = await _routeRepository.GetByIdAsync(ticketTypeModel.RouteId);
                if (route == null)
                {
                    throw new KeyNotFoundException("Route ID không tồn tại");
                }
                var map = _mapper.Map<TicketType>(ticketTypeModel);
                //var createdTicketType = await _ticketTypeRepository.CreateTicketTypeAsync(map);
                var createdTicketType = await _ticketTypeRepository.AddAsync(map);
                var result = _mapper.Map<CreateTicketTypeModel>(createdTicketType);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pagination<TicketTypeModel>> GetAllTicketType(PaginationParameter paginationParameter, int? routeId)
        {
            //var ticketTypes = await _ticketTypeRepository.ToPagination(paginationParameter);
            var ticketTypes = await _ticketTypeRepository.GetAllTicketType(paginationParameter, routeId);
            var routeIds = ticketTypes.Select(w => w.RouteId).Where(id => id.HasValue).Select(id => id.Value).ToList();

            var routes = await _routeRepository.GetRoutesByIdsAsync(routeIds);

            var ticketTypeModels = ticketTypes.Select(ticketType =>
            {
                var route = routes.FirstOrDefault(c => c.Id == ticketType.RouteId);
                var ticketTypeModel = _mapper.Map<TicketTypeModel>(ticketType);
                if (route != null)
                {
                    ticketTypeModel.RouteName = route.Name;
                    ticketTypeModel.RouteId = route.Id;
                }
                return ticketTypeModel;
            }).ToList();

            return new Pagination<TicketTypeModel>(ticketTypeModels,
                ticketTypes.TotalCount,
                ticketTypes.CurrentPage,
                ticketTypes.PageSize);
        }

        public async Task<TicketTypeModel> GetTicketTypeById(int id)
        {
            var route = await _routeRepository.GetByIdAsync(id);
            if (route != null)
            {
                var ticketType = await _ticketTypeRepository.GetTicketTypeByIdAsync(id);
                TicketTypeModel ticketTypeModel = _mapper.Map<TicketTypeModel>(ticketType);
                ticketTypeModel.RouteName = route.Name;
                return ticketTypeModel;
            }
            return null;
        }

        public async Task<bool> UpdateTicketTypeAsync(int id, UpdateTicketTypeModel ticketTypeModel)
        {
            try
            {
                var existingTicketType = await _ticketTypeRepository.GetTicketTypeByIdAsync(id);
                if (existingTicketType == null)
                {
                    return false;
                }

                _mapper.Map(ticketTypeModel, existingTicketType);
                await _ticketTypeRepository.UpdateAsync(existingTicketType);
                return true;
            }
            catch
            {
                throw new Exception("Xảy ra lỗi khi cập nhật loại vé");
            }
        }
    }
}
