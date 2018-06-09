namespace Marcus.Bus
{
    public interface IMetricService
    {
        MetricItem StartForType(object objForType, string group);
        MetricItem Start(string name, string group);
    }
}