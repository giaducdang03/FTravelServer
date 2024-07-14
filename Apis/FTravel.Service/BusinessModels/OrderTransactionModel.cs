using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FTravel.Service.BusinessModels
{
    public class OrderTransactionModel
    {
        public string? TransactionType { get; set; }
        public int? Amount { get; set; }
        [FromQuery(Name = "transaction-description")]
        public string? Description { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? Status { get; set; }
        public int? AccountBalance { get; set; }
    }
}
