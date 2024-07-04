using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels.OrderModels
{
    public class ResponseOrderModel : BaseEntity
    {
        public string Code { get; set; } = null!;

        public int? TotalPrice { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? PaymentStatus { get; set; }

        public int? CustomerId { get; set; }
    }
}
