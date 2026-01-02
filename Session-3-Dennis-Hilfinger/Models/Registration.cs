using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public int RunnerId { get; set; }

    public DateTime RegistrationDateTime { get; set; }

    public string RaceKitOptionId { get; set; } = null!;

    public byte RegistrationStatusId { get; set; }

    public decimal Cost { get; set; }

    public int CharityId { get; set; }

    public decimal SponsorshipTarget { get; set; }

    public virtual Charity Charity { get; set; } = null!;

    public virtual RaceKitOption RaceKitOption { get; set; } = null!;

    public virtual ICollection<RegistrationEvent> RegistrationEvents { get; set; } = new List<RegistrationEvent>();

    public virtual RegistrationStatus RegistrationStatus { get; set; } = null!;

    public virtual Runner Runner { get; set; } = null!;

    public virtual ICollection<Sponsorship> Sponsorships { get; set; } = new List<Sponsorship>();
}
