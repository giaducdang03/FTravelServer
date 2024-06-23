using FTravel.Repository.EntityModels;
using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class OrderModel
    {
        public int TotalPrice { get; set; }

        public int CustomerId { get; set; }

        public TransactionStatus PaymentStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
