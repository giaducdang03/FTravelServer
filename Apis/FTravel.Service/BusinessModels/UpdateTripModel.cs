using System.ComponentModel.DataAnnotations;

namespace FTravel.Service.BusinessModels
{
    public class UpdateTripModel
    {
        [Required]
        public string Name { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? ActualEndDate { get; set; }

        [Required]
        public string Status { get; set; }

        public int? DriverId { get; set; }

        public bool? IsTemplate { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one TicketTypeId is required.")]
        public List<int> TicketTypeIds { get; set; }

        
        public List<TripServiceModel> TripServices { get; set; }
    }
}