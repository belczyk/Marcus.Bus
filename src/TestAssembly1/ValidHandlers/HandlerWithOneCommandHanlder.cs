using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.ValidHandlers
{
    public class HandlerWithOneCommandHanlder : IHandler
    {
        public HandlerWithOneCommandHanlder(IBus bus)
        {
            Bus = bus;
        }

        private IBus Bus { get; }


        public async Task Handle(Command1 command)
        {
            await Bus.Publish(new Command1ExecutedEvent(), command);
        }
    }
}