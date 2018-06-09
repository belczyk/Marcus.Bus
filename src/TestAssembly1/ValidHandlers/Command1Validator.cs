using System.Threading.Tasks;
using Marcus.Bus.Abstractions;
using Marcus.Validation;

namespace TestAssembly1.ValidHandlers
{
    public class Command1Validator : ICommandValidator<Command1>
    {
        public Task<ValidationResult> Validate(Command1 command)
        {
            return Task.FromResult(new ValidationResult());
        }
    }
}