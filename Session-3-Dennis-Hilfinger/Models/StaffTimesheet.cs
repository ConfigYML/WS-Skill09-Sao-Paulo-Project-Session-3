using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class StaffTimesheet
{
    public int TimesheetId { get; set; }

    public int StaffId { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string? PayAmount { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
