using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Route : BaseEntity
{
    public string? UnsignName { get; set; }

    public string Name { get; set; } = null!;

    public int? StartPoint { get; set; }

    public int? EndPoint { get; set; }

    public string? Status { get; set; }

    public int? BusCompanyId { get; set; }

    public virtual BusCompany? BusCompany { get; set; }

    public virtual City? EndPointNavigation { get; set; }

    public virtual ICollection<RouteStation> RouteStations { get; set; } = new List<RouteStation>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual City? StartPointNavigation { get; set; }

    public virtual ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
