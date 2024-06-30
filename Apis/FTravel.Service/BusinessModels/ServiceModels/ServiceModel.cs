using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.ServiceModels
{
    public class ServiceModel : BaseEntity
    {
        public int? RouteId { get; set; }

        public string RouteName { get; set; } = "";

        public int? StationId { get; set; }

        public string StationName { get; set; } = "";

        public string Name { get; set; } = null!;

        public int? DefaultPrice { get; set; }

        public string? ImgUrl { get; set; }

        public string? ShortDescription { get; set; }

        public string? FullDescription { get; set; }
    }
}
