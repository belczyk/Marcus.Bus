using System;

namespace Marcus.Bus
{
    public abstract class Event
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public string SessionId { get; set; }
        public string RequestId { get; set; }
        public Guid TenantId { get; set; }
        public Guid PublishedBy { get; set; }
        public DateTime PublishedUTC { get; set; }
        public Guid? SourceCommandId { get; set; }
        public Guid? SourceEventId { get; set; }
    }
}