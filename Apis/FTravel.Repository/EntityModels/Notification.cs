using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Notification : BaseEntity
{
    public int? UserId { get; set; }

    public string? Type { get; set; }

    public bool? IsRead { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public virtual User? User { get; set; }
}
