using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.ValidHandlers
{
    public class EventHandler : IHandler
    {
        public async Task Handle(EventWithEventSource @event)
        {
        }
    }
}