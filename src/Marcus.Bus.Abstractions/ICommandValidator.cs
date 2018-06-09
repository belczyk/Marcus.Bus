using System.Threading.Tasks;
using Marcus.Validation;

namespace Marcus.Bus.Abstractions
{
    public interface ICommandValidator<in T> where T : Command
    {
        Task<ValidationResult> Validate(T command);
    }
}