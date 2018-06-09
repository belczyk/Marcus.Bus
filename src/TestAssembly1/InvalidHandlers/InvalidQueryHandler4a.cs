using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidQueryHandler4a : IHandler
    {
        public Task<int> Handle(Query1Query query)
        {
            return Task.FromResult(0);
        }
    }
}