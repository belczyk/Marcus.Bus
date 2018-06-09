using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.ValidHandlers
{
    public class HandlerWithOneQueryHandler : IHandler
    {
        public Task<int> Handle(Query1Query query)
        {
            return Task.FromResult(query.Value);
        }
    }
}