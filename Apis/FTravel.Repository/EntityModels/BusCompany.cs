using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class BusCompany : BaseEntity
{
    public string? UnsignName { get; set; }

    public string Name { get; set; } = null!;

    public string? ImgUrl { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public string ManagerEmail { get; set; } = null!;

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();

    public virtual ICollection<Station> Stations { get; set; } = new List<Station>();
}
