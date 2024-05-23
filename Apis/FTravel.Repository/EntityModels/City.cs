using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class City : BaseEntity
{
    public string? UnsignName { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<Route> RouteEndPointNavigations { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteStartPointNavigations { get; set; } = new List<Route>();
}
