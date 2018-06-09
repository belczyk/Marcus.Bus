using Marcus.Bus;

namespace TestAssembly1
{
    public class CommandWithValidation : Command
    {
        public CommandWithValidation()
        {
        }

        public CommandWithValidation(bool flag)
        {
            Flag = flag;
        }

        public bool Flag { get; set; }

        protected override void ValidateCommand()
        {
            IsTrue(Flag, nameof(Flag));
        }
    }
}