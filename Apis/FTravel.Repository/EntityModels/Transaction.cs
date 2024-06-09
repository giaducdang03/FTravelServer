using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Transaction : BaseEntity
{
    public int WalletId { get; set; }

    public string TransactionType { get; set; } = "";

    public int Amount { get; set; }

    public string? Description { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string Status { get; set; } = "";

    public int? OrderId { get; set; }

    public virtual Wallet? Wallet { get; set; }

    public virtual Order? Order { get; set; }

}
