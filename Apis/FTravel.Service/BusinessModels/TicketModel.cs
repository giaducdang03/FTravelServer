using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class TicketModel : BaseEntity
    {
        public int Id { get; set; }
        public int? TripId { get; set; }
        public string? SeatCode { get; set; }
        public string? Status { get; set; }
        public int? TicketTypeId { get; set; }
        public string? TicketTypeName { get; set; }
    }
}
