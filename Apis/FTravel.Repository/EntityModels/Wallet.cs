using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Wallet : BaseEntity
{
    public int? CustomerId { get; set; }

    public int? AccountBalance { get; set; }

    public string? Status { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
