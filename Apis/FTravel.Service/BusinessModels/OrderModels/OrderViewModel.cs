using FTravel.Repository.EntityModels;
using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class OrderViewModel : BaseEntity
    {
        public string Code { get; set; }
        public DateTime PaymentDate { get; set; }

        public string PaymentOrderStatus { get; set; }

        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public int? TotalPrice { get; set; }
        public string Email {  get; set; }

        public virtual List<OrderDetailModel> OrderDetail { get; set; } = new List<OrderDetailModel>();
        public virtual OrderTransactionModel? Transaction { get; set; }

    }
}
