using Marcus.Bus;

namespace TestAssembly1.InvalidHandlers
{
    public class InvalidQueryHandler3 : IHandler
    {
        public int Handle(Query1Query query)
        {
            return 0;
        }
    }
}