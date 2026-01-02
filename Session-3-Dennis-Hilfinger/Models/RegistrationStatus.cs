using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class RegistrationStatus
{
    public byte RegistrationStatusId { get; set; }

    public string RegistrationStatus1 { get; set; } = null!;

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
