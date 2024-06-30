using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.RouteModels
{
    public class CreateRouteModel
    {

        public string Name { get; set; } = "";

        public int StartPoint { get; set; }

        public int EndPoint { get; set; }

        public int? BusCompanyId { get; set; }
    }
}
