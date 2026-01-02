using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Gender
{
    public string Gender1 { get; set; } = null!;

    public virtual ICollection<Runner> Runners { get; set; } = new List<Runner>();

    public virtual ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
}
