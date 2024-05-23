using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class User : BaseEntity
{
    public string Email { get; set; } = null!;

    public bool? ConfirmEmail { get; set; } = false;

    public string? PasswordHash { get; set; }

    public string? UnsignFullName { get; set; }

    public string FullName { get; set; } = null!;

    public DateTime? Dob { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public int? Gender { get; set; }

    public string? Status { get; set; }

    public string? AvatarUrl { get; set; }

    public string? GoogleId { get; set; }

    public string? Fcmtoken { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Role? Role { get; set; }
}
