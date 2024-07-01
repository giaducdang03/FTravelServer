using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class ServiceTicket : BaseEntity
{
    public int? ServiceId { get; set; }

    public int? TicketId { get; set; }

    public int? Price { get; set; }

    public int? Quantity { get; set; }

    public virtual Service? Service { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
