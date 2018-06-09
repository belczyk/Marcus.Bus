using System.Threading.Tasks;
using Marcus.Bus;
using Marcus.Bus.Abstractions;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidMultiEventHandler : IHandler
    {
        [Handles(typeof(Command1ExecutedEvent))]
        [Handles(typeof(Command2ExecutedEvent))]
        public async Task Handle(DummyType1 c)
        {
        }
    }
}