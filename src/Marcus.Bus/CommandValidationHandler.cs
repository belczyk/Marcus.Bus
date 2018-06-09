using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Marcus.Validation;

namespace Marcus.Bus
{
    public class CommandValidationHandler
    {
        public CommandValidationHandler(Type commandType, Type validatorType)
        {
            CommandType = commandType;
            ValidatorType = validatorType;
            ValidateMethod = ValidatorType.GetMethods()
                .First(x => x.IsPublic && x.Name == "Validate" && x.ReturnType == typeof(Task<ValidationResult>) &&
                            x.GetParameters().First().ParameterType == CommandType);
        }

        private Type CommandType { get; }
        private Type ValidatorType { get; }

        private MethodInfo ValidateMethod { get; }

        public Task<ValidationResult> Validate(Command command, IServiceProvider serviceProvider)
        {
            var validator = serviceProvider.GetService(ValidatorType);

            var res = ValidateMethod.Invoke(validator, new object[] {command});

            return (Task<ValidationResult>) res;
        }
    }
}