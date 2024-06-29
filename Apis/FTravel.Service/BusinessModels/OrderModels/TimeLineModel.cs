using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class TimeLineModel
    {
        public DateTime? CreateDate { get; set; }
        public string? StartPoint { get; set; }
        public string? EndPoint { get; set; }
        public string? BusCompanyName { get; set; }
        public string? Name { get; set; }
    }
}
