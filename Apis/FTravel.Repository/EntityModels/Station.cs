using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Station : BaseEntity
{
    public string? UnsignName { get; set; }

    public string Name { get; set; } = null!;

    public int? BusCompanyId { get; set; }

    public string? Status { get; set; }

    public virtual BusCompany? BusCompany { get; set; }

    public virtual ICollection<RouteStation> RouteStations { get; set; } = new List<RouteStation>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
