using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Service : BaseEntity
{
    public int? RouteId { get; set; }

    public int? StationId { get; set; }

    public string Name { get; set; } = null!;

    public int? DefaultPrice { get; set; }

    public string? ImgUrl { get; set; }

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public virtual Route? Route { get; set; }

    public virtual ICollection<ServiceTicket> ServiceTickets { get; set; } = new List<ServiceTicket>();

    public virtual Station? Station { get; set; }

    public virtual ICollection<TripService> TripServices { get; set; } = new List<TripService>();
}
