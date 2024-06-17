using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class CreateRouteModel
    {

        public string Name { get; set; } = "";

        public int? StartPoint { get; set; }

        public int? EndPoint { get; set; }

        public string? Status { get; set; }

        public int? BusCompanyId { get; set; }
    }
}
