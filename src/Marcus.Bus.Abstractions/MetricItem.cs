using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Marcus.Bus
{
    public class MetricItem
    {
        public MetricItem(Func<string, decimal, Task> recordMetric)
        {
            RecordMetric = recordMetric;
            Stopwatch = Stopwatch.StartNew();
        }

        public Func<string, decimal, Task> RecordMetric { get; }
        public Stopwatch Stopwatch { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }

        public void Stop()
        {
            Stopwatch.Stop();
            var saveTask = RecordMetric(Name, Stopwatch.ElapsedMilliseconds);
            saveTask.Wait();
        }
    }
}