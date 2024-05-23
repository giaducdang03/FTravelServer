using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Trip : BaseEntity
{
    public string? UnsignName { get; set; }

    public string Name { get; set; } = null!;

    public int? RouteId { get; set; }

    public DateTime? OpenTicketDate { get; set; }

    public DateTime? EstimatedStartDate { get; set; }

    public DateTime? EstimatedEndDate { get; set; }

    public DateTime? ActualStartDate { get; set; }

    public DateTime? ActualEndDate { get; set; }

    public string? Status { get; set; }

    public bool? IsTemplate { get; set; }

    public int? DriverId { get; set; }

    public virtual Route? Route { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<TripService> TripServices { get; set; } = new List<TripService>();

    public virtual ICollection<TripTicketType> TripTicketTypes { get; set; } = new List<TripTicketType>();
}
