using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Country
{
    public string CountryCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public string CountryFlag { get; set; } = null!;

    public virtual ICollection<Marathon> Marathons { get; set; } = new List<Marathon>();

    public virtual ICollection<Runner> Runners { get; set; } = new List<Runner>();

    public virtual ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
}
