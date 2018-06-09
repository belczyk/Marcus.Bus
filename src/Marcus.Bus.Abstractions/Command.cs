using System;
using Marcus.Validation;

namespace Marcus.Bus
{
    public abstract class Command : ObjectWithValidation
    {
        public Guid CommandId { get; set; } = Guid.NewGuid();

        public string SessionId { get; set; }
        public string RequestId { get; set; }
        public Guid TenantId { get; set; }
        public Guid PublishedBy { get; set; }
        public DateTime PublishedUTC { get; set; }

        public override ValidationResult Validate()
        {
            NotNullOrWhiteSpace(SessionId, nameof(SessionId));
            NotNullOrWhiteSpace(RequestId, nameof(RequestId));
            NotDefault(TenantId, nameof(TenantId));
            NotDefault(PublishedBy, nameof(PublishedBy));
            NotDefault(PublishedUTC, nameof(PublishedUTC));
            NotDefault(CommandId, nameof(Command));

            ValidateCommand();

            return ValidationResult;
        }

        protected virtual void ValidateCommand()
        {
        }
    }
}