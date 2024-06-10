using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class StationModel
    {
        public int Id { get; set; }
        public string? UnsignName { get; set; }

        public string Name { get; set; } = null!;

        public int? BusCompanyId { get; set; }

        public string? Status { get; set; }
    }
}
