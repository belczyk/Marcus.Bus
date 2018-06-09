using System;

namespace Marcus.Bus
{
    public class MissingQueryHandlerException : Exception
    {
        public MissingQueryHandlerException(string query) : base("Missing query handler for: " + query)
        {
            Query = query;
        }

        public string Query { get; set; }
    }
}