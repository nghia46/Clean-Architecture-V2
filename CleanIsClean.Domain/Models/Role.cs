using System;
using System.Collections.Generic;

namespace CleanIsClean.Domain.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
