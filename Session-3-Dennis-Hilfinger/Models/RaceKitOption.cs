using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class RaceKitOption
{
    public string RaceKitOptionId { get; set; } = null!;

    public string RaceKitOption1 { get; set; } = null!;

    public decimal Cost { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
