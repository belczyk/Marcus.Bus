using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidCommandHandler2 : IHandler
    {
        public Task Handle(Command1 command)
        {
            return Task.CompletedTask;
        }
    }
}