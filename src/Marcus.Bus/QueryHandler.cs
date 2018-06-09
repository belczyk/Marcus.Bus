using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Marcus.Bus
{
    public class QueryHandler
    {
        public QueryHandler(Type queryType, MethodInfo methodInfo)
        {
            DelegateHandlerType = methodInfo.DeclaringType;
            MethodInfo = methodInfo;
            QueryType = queryType;
        }

        public Type DelegateHandlerType { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public Type QueryType { get; set; }
        public string Name => $"{DelegateHandlerType.FullName}.{MethodInfo.Name}";

        public async Task<T> Execute<T>(Query<T> query, IServiceProvider serviceProvider)
        {
            var handler = serviceProvider.GetService(DelegateHandlerType);
            var resultTask = (Task<T>) MethodInfo.Invoke(handler, new object[] {query});
            var res = await resultTask;

            return res;
        }
    }
}