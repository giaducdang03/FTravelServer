using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class TripService : BaseEntity
{
    public int? TripId { get; set; }

    public int? ServiceId { get; set; }

    public int? ServicePrice { get; set; }

    public virtual Service? Service { get; set; }

    public virtual Trip? Trip { get; set; }
}
