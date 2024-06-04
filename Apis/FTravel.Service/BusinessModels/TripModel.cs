using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class TripModel
    {
        public int Id { get; set; }
        public string? UnsignName { get; set; }
        public string Name { get; set; }
        public string? RouteName { get; set; } 
        public DateTime? OpenTicketDate { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string? Status { get; set; }
        public bool? IsTemplate { get; set; }
        public int? DriverId { get; set; }
        public List<TicketModel> Tickets {  get; set; } 
    }
}
