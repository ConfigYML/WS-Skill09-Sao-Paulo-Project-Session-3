using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public int? PositionId { get; set; }

    public string Email { get; set; } = null!;

    public string? Comments { get; set; }

    public virtual StaffPosition? Position { get; set; }

    public virtual ICollection<StaffTimesheet> StaffTimesheets { get; set; } = new List<StaffTimesheet>();
}
