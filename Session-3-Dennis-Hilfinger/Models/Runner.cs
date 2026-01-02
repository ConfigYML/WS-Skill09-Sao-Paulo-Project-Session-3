using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Runner
{
    public int RunnerId { get; set; }

    public string Email { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string CountryCode { get; set; } = null!;

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual User EmailNavigation { get; set; } = null!;

    public virtual Gender GenderNavigation { get; set; } = null!;

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
