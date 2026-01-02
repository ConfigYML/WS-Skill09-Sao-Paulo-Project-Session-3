using System;
using System.Collections.Generic;

namespace Session_3_Dennis_Hilfinger;

public partial class EventType
{
    public string EventTypeId { get; set; } = null!;

    public string EventTypeName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
