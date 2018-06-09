using System;

namespace Marcus.Bus
{
    public class CommandCannotBeNullException : Exception
    {
        public CommandCannotBeNullException(string name)
            : base("Command name " + name)
        {
        }
    }
}