using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class TripTicketType : BaseEntity
{
    public int? TripId { get; set; }

    public int? TicketTypeId { get; set; }

    public virtual TicketType? TicketType { get; set; }

    public virtual Trip? Trip { get; set; }
}
