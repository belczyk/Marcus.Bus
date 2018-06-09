using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.ValidHandlers
{
    public class HandlerWithOneEventHandler : IHandler
    {
        public HandlerWithOneEventHandler(IBus bus)
        {
            Bus = bus;
        }

        private IBus Bus { get; }

        public Task Hanlde(Command1ExecutedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}