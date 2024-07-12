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
                OrderId = x.Order.Id,
                TickerId = x.Ticket.Id,
                CustomerId = x.Order.CustomerId,
                TripId = x.Ticket.Trip.Id,
                TotalPrice = x.Order.TotalPrice,
                ActualEndDate = x.Ticket.Trip.ActualEndDate,
                ActualStartDate = x.Ticket.Trip.ActualStartDate,
                BuscompanyName = x.Ticket.Trip.Route.BusCompany.Name,
                StartPointName = x.Ticket.Trip.Route.EndPointNavigation.Name,
                EndPointName = x.Ticket.Trip.Route.StartPointNavigation.Name,
                UnsignNameTrip = x.Ticket.Trip.UnsignName,
                NameTrip = x.Ticket.Trip.Name,
                RouteId = x.Ticket.Trip.RouteId,
                OpenTicketDate = x.Ticket.Trip.OpenTicketDate,
                EstimatedStartDate = x.Ticket.Trip.OpenTicketDate,
                EstimatedEndDate = x.Ticket.Trip.OpenTicketDate,
                Status = x.Ticket.Trip.Status,
                IsTemplate = x.Ticket.Trip.IsTemplate,
                DriverId = x.Ticket.Trip.DriverId,
                ImgUrl = x.Ticket.Trip.Route.BusCompany.ImgUrl

            }).ToList();

            return routeModels;
        }
    }
}
