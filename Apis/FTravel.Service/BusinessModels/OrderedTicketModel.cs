using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class OrderedTicketModel
    {
        public int Id { get; set; }
        public string? SeatCode { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? ActualEndDate { get; set; }
        public string? StartPointName { get; set; }

        public string? EndPointName { get; set; }
        public string BuscompanyName { get; set; } = null!;
    }
}
