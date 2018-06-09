using System;

namespace Marcus.Bus
{
    public class EventProcessingFailed : Event
    {
        public EventProcessingFailed(Exception exception, Event @event)
        {
            Exception = exception;
            Event = @event;
        }

        public EventProcessingFailed()
        {
        }

        public Exception Exception { get; set; }
        public Event Event { get; set; }
    }
}