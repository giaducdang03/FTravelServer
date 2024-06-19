using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class RouteModel : BaseEntity
    {
        public string? UnsignName { get; set; }

        public string Name { get; set; } = null!;

        public string? StartPoint { get; set; }

        public string? EndPoint { get; set; }

        public string? Status { get; set; }

        public string? BusCompanyName { get; set; }

        public virtual BuscompanyModel? BusCompany { get; set; }

        public virtual ICollection<RouteStation> RouteStations { get; set; } = new List<RouteStation>();

        public virtual ICollection<Repository.EntityModels.Service> Services { get; set; } = new List<Repository.EntityModels.Service>();
    }
}
