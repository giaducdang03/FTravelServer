using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class GetAllOrderModel
    {
        public string OrderId { get; set; }
        public string OrderCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PaymentDate { get; set; }

        public string PaymentOrderStatus { get; set; }

        public string CustomerName { get; set; }
        public string TripName { get; set; }
        public string Phone { get; set; }
        public int? TotalPrice { get; set; }
        public string Email { get; set; }
        public string BusCompanyName { get; set; }
    }
}
