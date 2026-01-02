using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Charity
{
    public int CharityId { get; set; }

    public string CharityName { get; set; } = null!;

    public string? CharityDescription { get; set; }

    public string? CharityLogo { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
