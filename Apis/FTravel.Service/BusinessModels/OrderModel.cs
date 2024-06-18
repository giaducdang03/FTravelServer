using FTravel.Repository.EntityModels;
using FTravel.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class OrderModel
    {
        public int TotalPrice { get; set; }

        public int CustomerId { get; set; }

        public TransactionStatus PaymentStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public class OrderedTicketModel
    {
        public int Id { get; set; }
        public string? SeatCode { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? ActualEndDate { get; set; }
        public string? StartPointName { get; set; }

        public string? EndPointName { get; set; }
        public string BuscompanyName { get; set; } = null!;

    }
}
