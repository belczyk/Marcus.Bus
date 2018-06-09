using System.Threading.Tasks;
using Marcus.Bus;
using Marcus.Bus.Abstractions;

namespace TestAssembly1.ValidHandlers
{
    public class MultiEventHandler : IHandler
    {
        [Handles(typeof(Command2ExecutedEvent))]
        [Handles(typeof(Command3ExecutedEvent))]
        public async Task Handle(Event c)
        {
        }
    }
}