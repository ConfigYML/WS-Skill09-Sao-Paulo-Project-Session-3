using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Marathon
{
    public byte MarathonId { get; set; }

    public string MarathonName { get; set; } = null!;

    public string? CityName { get; set; }

    public string CountryCode { get; set; } = null!;

    public short? YearHeld { get; set; }

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
