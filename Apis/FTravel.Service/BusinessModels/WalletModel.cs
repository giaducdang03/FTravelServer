using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class WalletModel
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public string CustomerName { get; set; } = "";

        public int? AccountBalance { get; set; }

        public string? Status { get; set; }
    }
}
