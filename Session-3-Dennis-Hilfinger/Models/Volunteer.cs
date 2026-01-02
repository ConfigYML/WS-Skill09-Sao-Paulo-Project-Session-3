using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Volunteer
{
    public int VolunteerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string CountryCode { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual Gender GenderNavigation { get; set; } = null!;
}
