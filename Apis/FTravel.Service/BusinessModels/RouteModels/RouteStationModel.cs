using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.StationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.RouteModels
{
    public class RouteStationModel : BaseEntity
    {
        public int? RouteId { get; set; }

        public int? StationId { get; set; }

        public int? StationIndex { get; set; }

        public virtual StationModel? Station { get; set; }
    }
}
