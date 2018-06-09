using Marcus.Bus;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidCommandHandler3 : IHandler
    {
        public int Handle(Command1 command)
        {
            return 1;
        }
    }
}