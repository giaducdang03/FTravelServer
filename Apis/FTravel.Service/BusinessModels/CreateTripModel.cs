using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class CreateTripModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int RouteId { get; set; }

        public DateTime? OpenTicketDate { get; set; }

        public DateTime? EstimatedStartDate { get; set; }

        public DateTime? EstimatedEndDate { get; set; }

        public string? Status { get; set; }

        public bool? IsTemplate { get; set; }

        public int? DriverId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one TicketTypeId is required.")]
        public List<int> TicketTypeIds { get; set; }
        public List<TripServiceModel> TripServices { get; set; }
    }
}
