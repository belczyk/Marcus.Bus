using System;

namespace Marcus.Bus.Tests
{
    public class FakeServiceProvider : IServiceProvider
    {
        private static IBus _bus;

        private static IBus Bus => _bus ?? (_bus = new Bus(new FakeServiceProvider(), new StubBusSession(),
                                       new StubCommandsEventsStore(), new StubMetricsService(), new Time()));

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IBus)) return Bus;

            var defaultConstructor = serviceType.GetConstructor(Type.EmptyTypes);
            if (defaultConstructor != null) return defaultConstructor.Invoke(null);

            var busConstructor = serviceType.GetConstructor(new[] {typeof(IBus)});

            return busConstructor.Invoke(new object[] {Bus});
        }
    }
}