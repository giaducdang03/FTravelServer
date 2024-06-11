using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class CityModel : BaseEntity
    {
        public string? UnsignName { get; set; }
        public string Name { get; set; } = null!;
        public int Code { get; set; }
    }
}
