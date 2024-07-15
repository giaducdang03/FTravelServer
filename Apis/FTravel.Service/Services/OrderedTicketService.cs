using AutoMapper;
using FTravel.Repositories.Commons;
using FTravel.Repository.Commons;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.OrderModels;
using FTravel.Service.Enums;
using FTravel.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class OrderedTicketService : IOrderedTicketService
    {
        private readonly IOrderedTicketRepository _orderedTicketRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public OrderedTicketService(
            IOrderedTicketRepository orderedTicketRepository,
            ICustomerRepository customerRepository,
            ITicketRepository ticketRepository,
            IMapper mapper)
        {
            _orderedTicketRepository = orderedTicketRepository;
            _customerRepository = customerRepository;
            _ticketRepository = ticketRepository;
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
                //IsTemplate = x.Ticket.Trip.IsTemplate,
                DriverId = x.Ticket.Trip.DriverId,
                ImgUrl = x.Ticket.Trip.Route.BusCompany.ImgUrl

            }).ToList();

            return routeModels;
        }

        public async Task<Pagination<OrderTicketModel>> GetTicketByCustomer(string email, PaginationParameter paginationParameter)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            if (customer == null)
            {
                throw new Exception("Khách hàng không tồn tại.");
            }
            
            var orders = await _orderedTicketRepository.GetOrderedTicketsByCustomer(customer.Id, paginationParameter);

            if (orders.Any())
            {
                var returnModel = orders.Select(x => new OrderTicketModel
                {
                    OrderId = x.Id,
                    CustomerId = x.Order.CustomerId,
                    BuscompanyName = x.Ticket.Trip.Route.BusCompany.Name,
                    BuscompanyImg = x.Ticket.Trip.Route.BusCompany.ImgUrl,
                    StartPointName = x.Ticket.Trip.Route.EndPointNavigation.Name,
                    EndPointName = x.Ticket.Trip.Route.StartPointNavigation.Name,
                    EstimateStartDate = x.Ticket.Trip.EstimatedStartDate,
                    EstimateEndDate = x.Ticket.Trip.EstimatedEndDate,
                    TicketId = x.TicketId,
                    TripId = x.Ticket.TripId,
                    TripStatus = x.Ticket.Status,
                    TotalPrice = x.Order.TotalPrice
                }).ToList();

                var result = new Pagination<OrderTicketModel>(returnModel, orders.Count, paginationParameter.PageIndex, paginationParameter.PageSize);
                return result;
            }

            return new Pagination<OrderTicketModel>(new List<OrderTicketModel>(), 0, paginationParameter.PageIndex, paginationParameter.PageSize);

        }

        public async Task<OrderTicketModelDetails> GetTicketDetailCustomerById(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync2(ticketId);
            if (ticket != null)
            {
                // get customer
                var customer = await _orderedTicketRepository.GetCustomerByTicketBoughtId(ticket.Id);
                var customerModel = new CustomerTicketModel();
                if (customer != null) 
                { 
                    customerModel.CustomerId = customer.Id;
                    customerModel.CustomerEmail = customer.Email;
                    customerModel.CustomerName = customer.FullName;
                    customerModel.CustomerPhone = customer.PhoneNumber;
                }

                var returnModel = new OrderTicketModelDetails
                {
                    TicketId = ticket.Id,
                    TripId = ticket.TripId,
                    StartPointName = ticket.Trip.Route.StartPointNavigation.Name,
                    EndPointName = ticket.Trip.Route.EndPointNavigation.Name,
                    EstimateStartDate = ticket.Trip.EstimatedStartDate,
                    EstimateEndDate = ticket.Trip.EstimatedEndDate,
                    TripStatus = ticket.Trip.Status,
                    TicketStatus = ticket.Status,
                    SeatCode = ticket.SeatCode,
                    TicketTypeId = ticket.TicketTypeId,
                    TicketTypeName = ticket.TicketType.Name,
                    BuscompanyName = ticket.Trip.Route.BusCompany.Name,
                    BuscompanyImg = ticket.Trip.Route.BusCompany.ImgUrl,
                    CustomerTicketModel = customerModel,
                    ServiceTickets = _mapper.Map<List<ServiceTicketModel>>(ticket.ServiceTickets)
                };


                return returnModel;
            }
            return null;
        }
    }
}
