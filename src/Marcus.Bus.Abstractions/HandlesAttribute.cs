using System;

namespace Marcus.Bus.Abstractions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HandlesAttribute : Attribute
    {
        public HandlesAttribute(Type type)
        {
            Type = type;

            if (!typeof(Command).IsAssignableFrom(type) && !typeof(Event).IsAssignableFrom(type))
                throw new Exception(
                    $"Invalid Handles attribute, pass Event or Command as attributed (passed: {type.Namespace}.{type.Name})");
        }

        public Type Type { get; set; }
    }
}