using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class BuyTicketModel
    {
        public int TicketId { get; set; }

        public int CustomerId { get; set; }

        //public int TripId { get; set; }

        public List<TicketServiceModel>? Services { get; set; }
        
    }
}
