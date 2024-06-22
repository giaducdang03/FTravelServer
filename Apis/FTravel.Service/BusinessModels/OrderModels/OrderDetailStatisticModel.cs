using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class OrderDetailStatisticModel
    {
        public int? OrderDetailId { get; set; }
        public string ServiceName { get; set; }
        public int? UnitPrice { get; set; }
        public int? TotalPrice { get; set; }
        public int? Quantity { get; set; }
    }
}
