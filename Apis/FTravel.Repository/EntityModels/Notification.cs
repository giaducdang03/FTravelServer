using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Notification : BaseEntity
{
    public int UserId { get; set; }

    public string? Type { get; set; }

    public bool IsRead { get; set; } = false;

    public string? Title { get; set; }

    public string? Message { get; set; }

    public int? EntityId { get; set; }

    public virtual User? User { get; set; }
}
