using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Sponsorship
{
    public int SponsorshipId { get; set; }

    public string SponsorName { get; set; } = null!;

    public int RegistrationId { get; set; }

    public decimal Amount { get; set; }

    public virtual Registration Registration { get; set; } = null!;
}
