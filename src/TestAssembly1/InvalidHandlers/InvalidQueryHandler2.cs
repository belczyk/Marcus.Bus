using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidQueryHandler2 : IHandler
    {
        public Task Handle(Query1Query query)
        {
            return Task.CompletedTask;
        }
    }
}