using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class TicketServiceModel
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public int? Quantity { get; set; }

        //public int? Price { get; set; }


    }
}
