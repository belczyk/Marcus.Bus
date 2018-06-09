using System.Threading.Tasks;
using Marcus.Bus;
using Marcus.Bus.Abstractions;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidMultiCommandHandler : IHandler
    {
        [Handles(typeof(Command1))]
        [Handles(typeof(Command2))]
        public async Task Handle(DummyType1 c)
        {
        }
    }
}