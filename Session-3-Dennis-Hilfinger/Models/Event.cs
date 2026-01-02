using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class Event
{
    public string EventId { get; set; } = null!;

    public string EventName { get; set; } = null!;

    public string EventTypeId { get; set; } = null!;

    public byte MarathonId { get; set; }

    public DateTime? StartDateTime { get; set; }

    public decimal? Cost { get; set; }

    public short? MaxParticipants { get; set; }

    public virtual EventType EventType { get; set; } = null!;

    public virtual Marathon Marathon { get; set; } = null!;

    public virtual ICollection<RegistrationEvent> RegistrationEvents { get; set; } = new List<RegistrationEvent>();
}
