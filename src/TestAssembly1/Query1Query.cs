using Marcus.Bus;

namespace TestAssembly1
{
    public class Query1Query : Query<int>
    {
        public Query1Query()
        {
        }

        public Query1Query(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }
}