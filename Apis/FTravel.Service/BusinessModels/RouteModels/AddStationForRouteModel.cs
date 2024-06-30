using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.RouteModels
{
    public class AddStationForRouteModel
    {
        public int StationId { get; set; }

        public int RouteId { get; set; }

        public int StationIndex { get; set; }
    }
}
