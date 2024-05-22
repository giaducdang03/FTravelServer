using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class RouteStation : BaseEntity
{
    public int? RouteId { get; set; }

    public int? StationId { get; set; }

    public int? StationIndex { get; set; }

    public virtual Route? Route { get; set; }

    public virtual Station? Station { get; set; }
}
