using System;

namespace Marcus.Bus
{
    public interface IBusSession
    {
        Guid UserId { get; }
        Guid TenantId { get; }
        Guid? TenantIdSafe { get; }
        Guid? UserIdSafe { get; }
        string SessionId { get; }
        string RequestId { get; }
        DateTime Now { get; }
    }
}