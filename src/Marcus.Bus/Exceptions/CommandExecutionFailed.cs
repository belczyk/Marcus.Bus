using System;

namespace Marcus.Bus
{
    public class CommandExecutionFailed : Event
    {
        public CommandExecutionFailed(Exception exception, Command command)
        {
            Exception = exception;
            Command = command;
        }

        public CommandExecutionFailed()
        {
        }

        public Exception Exception { get; set; }
        public Command Command { get; set; }
    }
}