using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Customer : BaseEntity
{
    public string? UnsignFullName { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? Dob { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public int? Gender { get; set; }

    public int? TicketBought { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Wallet? Wallet { get; set; }

    //public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
