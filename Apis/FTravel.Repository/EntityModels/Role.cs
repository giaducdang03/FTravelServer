using System;
using System.Collections.Generic;

namespace FTravel.Repository.EntityModels;

public partial class Role : BaseEntity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
