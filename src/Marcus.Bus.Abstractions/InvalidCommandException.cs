using Marcus.Validation;

namespace Marcus.Bus.Abstractions
{
    public class InvalidCommandException : InvalidObjectException
    {
        public InvalidCommandException(ValidationResult validationResult) : base(validationResult)
        {
        }
    }
}