using System;

namespace Marcus.Bus
{
    public class EventCannotBeNullException : Exception
    {
        public EventCannotBeNullException(string name)
            : base("Event name " + name)
        {
        }
    }
}