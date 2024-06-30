using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.TicketModels
{
    public class TicketTypeModel : BaseEntity
    {
        public string Name { get; set; } = null!;

        public int? Price { get; set; }

        public int RouteId { get; set; }

        public string RouteName { get; set; } = "";
    }
}
