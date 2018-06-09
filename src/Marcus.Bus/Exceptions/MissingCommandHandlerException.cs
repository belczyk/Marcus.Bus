using System;

namespace Marcus.Bus
{
    public class MissingCommandHandlerException : Exception
    {
        public MissingCommandHandlerException(string command) : base("Missing command handler for: " + command)
        {
            Command = command;
        }

        public string Command { get; set; }
    }
}