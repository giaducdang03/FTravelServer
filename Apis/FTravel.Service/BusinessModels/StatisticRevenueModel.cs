using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class StatisticRevenueModel
    {
        public string? StatisticDate { get; set; }
        public string? BusCompanyName { get; set; }
        public int? TotalRevenue { get; set; }
        public int? TotalTicketSolds { get; set; }
        public List<OrderDetailStatisticModel> Orders { get; set; }
    }
}
