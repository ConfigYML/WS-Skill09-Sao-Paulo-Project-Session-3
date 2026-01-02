using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class StaffPosition
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public string? PositionDescription { get; set; }

    public string PayPeriod { get; set; } = null!;

    public string PayRate { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
