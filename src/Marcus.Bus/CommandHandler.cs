using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Marcus.Bus
{
    public class CommandHandler
    {
        public CommandHandler(Type commandType, MethodInfo methodInfo)
        {
            DelegateHandlerType = methodInfo.DeclaringType;
            MethodInfo = methodInfo;
            CommandType = commandType;
        }

        public Type DelegateHandlerType { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public Type CommandType { get; set; }
        public string Name => $"{DelegateHandlerType.FullName}.{MethodInfo.Name}";


        public Task Handle(Command command, IServiceProvider serviceProvider)
        {
            var typeInstance = serviceProvider.GetService(DelegateHandlerType);
            var res = MethodInfo.Invoke(typeInstance, new object[] {command});

            return (Task) res;
        }
    }
}