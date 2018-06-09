using System.Threading.Tasks;

namespace Marcus.Bus.Tests
{
    public class StubMetricsService : IMetricService
    {
        public MetricItem StartForType(object objForType, string group)
        {
            return new MetricItem((str, dec) => Task.CompletedTask);
        }

        public MetricItem Start(string name, string group)
        {
            return new MetricItem((str, dec) => Task.CompletedTask);
        }
    }
}