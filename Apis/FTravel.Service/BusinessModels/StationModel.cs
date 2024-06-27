using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class StationModel : BaseEntity
    {

        public int Id { get; set; }

        public string? UnsignName { get; set; }

        public string Name { get; set; } = "";

        public string BusCompanyName { get; set; } = "";

        public string? Status { get; set; }
    }
}
