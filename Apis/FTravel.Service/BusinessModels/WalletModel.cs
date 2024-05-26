using FTravel.Repository.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class WalletModel : BaseEntity
    {

        public int? CustomerId { get; set; }

        public string CustomerName { get; set; } = "";

        public int? AccountBalance { get; set; }

        public string? Status { get; set; }
    }
}
