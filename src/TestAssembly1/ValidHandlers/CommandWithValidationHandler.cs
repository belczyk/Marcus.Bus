using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.ValidHandlers
{
    public class CommandWithValidationHandler : IHandler
    {
        public async Task Handle(CommandWithValidation command)
        {
        }
    }
}