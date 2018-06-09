using System;

namespace Marcus.Bus
{
    public class Time : ITime
    {
        private TimeSpan? DeltaToReferenceTime { get; set; }

        public void OverrideTime(DateTime referenceNow)
        {
            DeltaToReferenceTime = referenceNow - DateTime.UtcNow;
        }

        public DateTime Now => DeltaToReferenceTime.HasValue
            ? DateTime.UtcNow + DeltaToReferenceTime.Value
            : DateTime.UtcNow;
    }
}