using System.Threading.Tasks;
using Marcus.Validation;

namespace Marcus.Bus.Abstractions
{
    public abstract class CommandValidtor<T> : ValidatorBase, ICommandValidator<T> where T : Command
    {
        public async Task<ValidationResult> Validate(T command)
        {
            await ValidateCommand(command);
            return ValidationResult;
        }

        protected abstract Task ValidateCommand(T command);
    }
}