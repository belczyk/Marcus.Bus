using System;

namespace Marcus.Bus
{
    public interface ITime
    {
        DateTime Now { get; }
        void OverrideTime(DateTime referenceNow);
    }
}