using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Pagination<TicketTypeModel>> GetAllTicketType(PaginationParameter paginationParameter)
        {
            var ticketTypes = await _ticketTypeRepository.ToPagination(paginationParameter);
            var routeIds = ticketTypes.Select(w => w.RouteId).Where(id => id.HasValue).Select(id => id.Value).ToList();

            var routes = await _routeRepository.GetRoutesByIdsAsync(routeIds);

            var ticketTypeModels = ticketTypes.Select(ticketType =>
            {
                var route = routes.FirstOrDefault(c => c.Id == ticketType.RouteId);
                var ticketTypeModel = _mapper.Map<TicketTypeModel>(ticketType);
                if (route != null)
                {
                    ticketTypeModel.RouteName = route.Name;
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
    }
}
