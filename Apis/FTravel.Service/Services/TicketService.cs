using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTravel.Service.BusinessModels;
using FTravel.Service.BusinessModels.TicketModels;
using FTravel.Service.Services.Interface;
using FTravel.Repository.Repositories.Interface;
using AutoMapper;
using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Service.BusinessModels.OrderModels;
using System.Net.Sockets;

namespace FTravel.Service.Services
{
    public class TicketService : ITicketService
    {
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper, IOrderRepository orderRepository)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<TicketModel> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetTripDetailById(id);
            TicketModel ticketModel = _mapper.Map<TicketModel>(ticket);
            return ticketModel;
        }

        public async Task<OrderModel> BuyTicket(BuyTicketModel model)
        {
            var ticket = await _ticketRepository.GetTripDetailById(model.TicketId);
            if (ticket == null)
            {
                throw new KeyNotFoundException("Vé không tồn tại");
            }

            if (model.Services != null)
            {
                foreach (var item in model.Services)
                {

                    if (await _orderRepository.CheckServiceInTrip(model.TripId, item.Id.Value) == false)
                    {
                        throw new Exception("Service not found in trip.");
                    }
                }
            }

            List<ServiceTicket> ticketModel = _mapper.Map<List<ServiceTicket>>(model.Services);

            foreach (var item in ticketModel)
            {
                ticket.ServiceTickets.Add(item);
            }
            
            var customer = await _customerRepository.GetByIdAsync(model.CustomerId);
            if(customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            OrderModel order = new OrderModel();
            order.CustomerId = model.CustomerId;
            OrderDetail ticketDetail = new OrderDetail();
            ticketDetail.TicketId = model.TicketId;
            ticketDetail.Quantity = 1;
            ticketDetail.UnitPrice = ticket.TicketType.Price;
            ticketDetail.Type = "Ticket";
            ticketDetail.OrderId = 0;
            
            order.OrderDetails.Add(ticketDetail);

            foreach (var item in model.Services)
            {
                OrderDetail serviceDetail = new OrderDetail();
                serviceDetail.TicketId = model.TicketId;
                serviceDetail.Quantity = item.Quantity;
                serviceDetail.UnitPrice = item.Price;
                serviceDetail.Type = "Service";
                serviceDetail.OrderId = 0;

                order.OrderDetails.Add(serviceDetail);
            }

            foreach (var item in order.OrderDetails)
            {
                order.TotalPrice += item.UnitPrice.Value * item.Quantity.Value;
            }


            return order;
        }
    }
}
