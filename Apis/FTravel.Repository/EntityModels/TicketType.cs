using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class TicketType : BaseEntity
{
    public string Name { get; set; } = null!;

    public int? RouteId { get; set; }

    public int? Price { get; set; }

    public virtual Route? Route { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<TripTicketType> TripTicketTypes { get; set; } = new List<TripTicketType>();
}
