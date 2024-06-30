using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.BuscompanyModels
{
    public class BuscompanyModel : BaseEntity
    {
        public string? UnsignName { get; set; }

        public string Name { get; set; } = null!;

        public string? ImgUrl { get; set; }

        public string? ShortDescription { get; set; }

        public string? FullDescription { get; set; }

        public string ManagerEmail { get; set; } = null!;
    }
}
