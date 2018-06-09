using System;

namespace Marcus.Bus.Tests
{
    public class StubBusSession : IBusSession
    {
        public Guid UserId { get; set; } = new Guid("5746E192-6BAD-4CBF-A828-0F0B00C4258F");
        public Guid TenantId { get; set; } = new Guid("277D2D0C-0502-49F9-A712-E022D96E49D1");
        public Guid? TenantIdSafe { get; set; }
        public Guid? UserIdSafe { get; set; }

        public string SessionId { get; set; } =
            new Guid("DB702DD5-C07A-4C1C-A98A-EB17386E6926").ToString().Substring(0, 8);

        public string RequestId { get; set; } =
            new Guid("A75FC3EF-9D1F-4C9F-B948-CE6B730114A5").ToString().Substring(0, 8);

        public DateTime Now { get; set; } = DateTime.Now;
    }
}