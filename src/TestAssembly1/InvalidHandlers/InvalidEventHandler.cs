using Marcus.Bus;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidEventHandler : IHandler
    {
        public int Handle(Command1ExecutedEvent @event)
        {
            return 0;
        }
    }
}