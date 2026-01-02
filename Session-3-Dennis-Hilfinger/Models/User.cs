using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class User
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string RoleId { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Runner> Runners { get; set; } = new List<Runner>();
}
