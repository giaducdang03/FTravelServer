using FTravel.Repository.EntityModels;
using FTravel.Service.BusinessModels.RouteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class StatisticRevenueModel
    {
       public int? TotalPrice { get; set; }
        public int? AmountOfUser { get; set; }
        public int? AmountOfOrder { get; set; }
        public List<ChartOrderModel>? ChartOrders { get; set; }
        public List<TimeLineModel>? TimeLine { get; set; }

    }
}
