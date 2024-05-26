using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class TransactionModel
    {
        public int Id { get; set; }

        public int? WalletId { get; set; }

        public string? TransactionType { get; set; }

        public int? Amount { get; set; }

        public string? Description { get; set; }

        public DateTime? TransactionDate { get; set; }

        public string? Status { get; set; }
    }
}
