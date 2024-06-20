using AutoMapper;
using FTravel.Repository.EntityModels;
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
    public class OrderedTicketService : IOrderedTicketService
    {
        private readonly IOrderedTicketRepository _orderedTicketRepository;
        private readonly IMapper _mapper;

        public OrderedTicketService(IOrderedTicketRepository orderedTicketRepository, IMapper mapper)
        {
            _orderedTicketRepository = orderedTicketRepository;
            _mapper = mapper;
        }
        public async Task<List<Order>> GetAllOrderedTicketByCustomerIdService(int customer)
        {
            var data = await _orderedTicketRepository.GetOrderedTicketListByCustomerId(customer);
            return data;
        }

        public async Task<List<OrderedTicketModel>> GetHistoryOfTripsTakenByCustomerIdService(int customer)
        {
            var data = await _orderedTicketRepository.GetHistoryOfTripsTakenByCustomerId(customer);
            if (!data.Any())
            {
                return null;
            }

            var routeModels = data.Select(x => new OrderedTicketModel
            {
                Id = x.Id,
                ActualEndDate = x.Ticket.Trip.ActualEndDate,
                ActualStartDate = x.Ticket.Trip.ActualStartDate,
                BuscompanyName = x.Ticket.Trip.Route.BusCompany.Name,
                StartPointName = x.Ticket.Trip.Route.EndPointNavigation.Name,
                EndPointName = x.Ticket.Trip.Route.StartPointNavigation.Name,

            }).ToList();

            return routeModels;
        }
    }
}
