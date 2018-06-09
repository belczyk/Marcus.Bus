using System.Threading.Tasks;
using Marcus.Bus;
using Marcus.Bus.Abstractions;

namespace TestAssembly1.ValidHandlers
{
    public class MultiCommandHandler : IHandler
    {
        [Handles(typeof(Command2))]
        [Handles(typeof(Command3))]
        public async Task Handle(Command c)
        {
        }
    }
}