using System.Threading.Tasks;
using Marcus.Bus;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidQueryHandler1 : IHandler
    {
        public Task<string> Handle(Query1Query query)
        {
            return Task.FromResult("abc");
        }
    }
}