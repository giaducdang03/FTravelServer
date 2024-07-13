using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class OrderedTicketModel
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? TickerId { get; set; }
        public int? TripId { get; set; }

        public string? SeatCode { get; set; }
        public int? TotalPrice { get; set; }

        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string? StartPointName { get; set; }
        public string? EndPointName { get; set; }
        public string BuscompanyName { get; set; } = null!;
        public string? ImgUrl { get; set; }

        public string? UnsignNameTrip { get; set; }
        public string NameTrip { get; set; } = null!;
        public int? RouteId { get; set; }

        public DateTime? OpenTicketDate { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }

        public string? Status { get; set; }
        //public bool? IsTemplate { get; set; }
        public int? DriverId { get; set; }
    }
}
