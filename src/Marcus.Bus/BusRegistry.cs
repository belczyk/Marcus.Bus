using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Marcus.Bus
{
    public class BusRegistry
    {
        private readonly Dictionary<Type, CommandValidationHandler> CommandValiators =
            new Dictionary<Type, CommandValidationHandler>();

        private Dictionary<Type, CommandHandler> CommandsHandlers { get; } = new Dictionary<Type, CommandHandler>();
        private Dictionary<Type, EventHandler> EventsHandlers { get; } = new Dictionary<Type, EventHandler>();
        private Dictionary<Type, QueryHandler> QueryHandlers { get; } = new Dictionary<Type, QueryHandler>();

        public void AddCommandHandler(Type commandType, MethodInfo handlerMethod)
        {
            var handler = new CommandHandler(commandType, handlerMethod);
            if (CommandsHandlers.ContainsKey(commandType))
                throw new InvalidOperationException(
                    $"Tried add second command handler. Hanlder1: {handler.Name}. Hander2: {CommandsHandlers[handler.CommandType].Name}");

            CommandsHandlers.Add(handler.CommandType, handler);
        }

        public void AddQueryHandler(Type queryType, MethodInfo handlerMethod)
        {
            var queryHandler = new QueryHandler(queryType, handlerMethod);

            if (QueryHandlers.ContainsKey(queryHandler.QueryType))
                throw new InvalidOperationException(
                    $"Tried add second command handler. Hanlder1: {queryHandler.Name}. Hander2: {QueryHandlers[queryHandler.QueryType].Name}");

            QueryHandlers.Add(queryHandler.QueryType, queryHandler);
        }

        public void AddCommandValidator(Type commandType, Type validatorType)
        {
            CommandValiators.Add(commandType, new CommandValidationHandler(commandType, validatorType));
        }

        public CommandHandler GetCommandHandler(Type commandType)
        {
            return CommandsHandlers[commandType];
        }

        public EventHandler GetEventHandler(Type eventType)
        {
            return EventsHandlers[eventType];
        }

        public QueryHandler GetQueryHandler(Type queryType)
        {
            return QueryHandlers[queryType];
        }

        public CommandValidationHandler GetValidator(Command c)
        {
            return CommandValiators[c.GetType()];
        }

        public void AddEventSubscriber(Type eventType, MethodInfo methodInfo)
        {
            if (!EventsHandlers.ContainsKey(eventType))
                EventsHandlers[eventType] = new EventHandler(eventType);
            EventsHandlers[eventType].AddSubscriber(methodInfo.DeclaringType, methodInfo);
        }

        public bool HasAnySubscribers<T>()
        {
            return EventsHandlers.ContainsKey(typeof(T));
        }

        public bool HasQueryHanlder(Type queryType)
        {
            return QueryHandlers.ContainsKey(queryType);
        }

        public bool HasCommandHanlder(Type commandKey)
        {
            return CommandsHandlers.ContainsKey(commandKey);
        }

        public bool HasValidator(Command command)
        {
            return CommandValiators.Keys.Any(x => x == command.GetType());
        }
    }
}