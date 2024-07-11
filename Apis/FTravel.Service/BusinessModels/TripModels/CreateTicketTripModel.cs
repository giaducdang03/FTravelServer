using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.TripModels
{
    public class CreateTicketTripModel
    {
        public string? SeatCode { get; set; }
        public string? Status { get; set; }
        public int? TicketTypeId { get; set; }
    }
}
