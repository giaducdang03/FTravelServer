using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class OrderDetail : BaseEntity
{
    public int? TicketId { get; set; }

    public int? OrderId { get; set; }

    public string? Type { get; set; }

    public string? ServiceName { get; set; }

    public int? UnitPrice { get; set; }

    public int? Quantity { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
