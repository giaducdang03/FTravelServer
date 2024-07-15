using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class OrderTicketModel
    {
        public int? OrderId { get; set; }
        public int? TicketId { get; set; }
        public int? TripId { get; set; }
        public string? TripStatus { get; set; }
        public int? TotalPrice { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? EstimateStartDate { get; set; }
        public DateTime? EstimateEndDate { get; set; }
        public string? StartPointName { get; set; }
        public string? EndPointName { get; set; }
        public string? BuscompanyName { get; set; }
        public string? BuscompanyImg { get; set; }
    }

    public class OrderTicketModelDetails : OrderTicketModel
    {
        public string? SeatCode { get; set; }
        public string? TicketStatus { get; set; }
        public int? TicketTypeId { get; set; }
        public string? TicketTypeName { get; set; }
        public CustomerTicketModel Customer { get; set; } = new CustomerTicketModel();
        public virtual ICollection<ServiceTicketModel> ServiceTickets { get; set; } = new List<ServiceTicketModel>();
    }

    public class TicketOrderModel
    {
        public int Id { get; set; }
        public int? TripId { get; set; }
        public string? SeatCode { get; set; }
        public int? Price { get; set; }
        public string? Status { get; set; }
        public int? TicketTypeId { get; set; }
        public string? TicketTypeName { get; set; }
        public virtual ICollection<ServiceTicketModel> ServiceTickets { get; set; } = new List<ServiceTicketModel>();

    }

    public class ServiceTicketModel
    {
        public int? ServiceId { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public string? ServiceName { get; set; }
        public int? StationId { get; set; }
        public string? StationName { get; set; }

    }

    public class CustomerTicketModel
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
    }
}
