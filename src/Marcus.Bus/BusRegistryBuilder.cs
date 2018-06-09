using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Marcus.Bus.Abstractions;

namespace Marcus.Bus
{
    public class BusRegistryBuilder
    {
        private static readonly Type HandlerType = typeof(IHandler);
        private static readonly Type CommandType = typeof(Command);
        private static readonly Type ValidatorType = typeof(ICommandValidator<>);
        private static readonly Type EventType = typeof(Event);
        private static readonly Type GenericType = typeof(Query<>);

        public BusRegistryBuilder(Func<Assembly, bool> assemblySelector, Func<Type, bool> typeSelector = null)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assemblySelector)
                .SelectMany(a => a.GetTypes())
                .Where(x => typeSelector == null || typeSelector(x));

            HandlerTypes = types
                .Where(p => !p.IsInterface && HandlerType.IsAssignableFrom(p));


            ValidtorTypes = types.Where(x =>
                x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ValidatorType));
        }

        public BusRegistryBuilder(params Type[] handlerTypes)
        {
            HandlerTypes = handlerTypes;
        }

        private IEnumerable<Type> HandlerTypes { get; }
        private IEnumerable<Type> ValidtorTypes { get; }

        public BusRegistry Bootstrap()
        {
            var registry = new BusRegistry();
            var methods = HandlerTypes.SelectMany(t => t.GetMethods().Where(m => m.IsPublic));
            foreach (var method in methods)
            {
                var mParams = method.GetParameters();
                if (mParams.Count() != 1) continue;

                var paramType = mParams.First().ParameterType;
                if (CommandType.IsAssignableFrom(paramType))
                    ProcessCommandHandlerMethod(method, paramType, registry);
                else if (EventType.IsAssignableFrom(paramType))
                    ProcessEventHanlderMethod(method, paramType, registry);
                else if (paramType.IsAssignableToGenericType(GenericType))
                    ProcessQueryHanlderMethod(method, paramType, registry);

                var handlesAttributes = method.GetCustomAttributes<HandlesAttribute>();
                if (handlesAttributes.Any()) ProcessMultiHandler(method, paramType, registry, handlesAttributes);
            }

            foreach (var validator in ValidtorTypes)
                registry.AddCommandValidator(
                    validator.GetInterfaces()
                        .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == ValidatorType)
                        .GenericTypeArguments.First(), validator);

            return registry;
        }


        private void ProcessMultiHandler(MethodInfo method, Type paramType, BusRegistry registry,
            IEnumerable<HandlesAttribute> handlesAttributes)
        {
            ValidateReturnTypeIsTask(method);

            foreach (var attr in handlesAttributes)
            {
                ValidateParameterIsAssignableFromType(paramType, attr.Type, method);
                if (EventType.IsAssignableFrom(paramType))
                    registry.AddEventSubscriber(attr.Type, method);
                else if (CommandType.IsAssignableFrom(paramType)) registry.AddCommandHandler(attr.Type, method);
            }
        }

        private void ValidateParameterIsAssignableFromType(Type entryType, Type type, MethodInfo methodInfo)
        {
            if (entryType.IsAssignableFrom(type))
                return;

            throw new Exception(
                $"Invalid paramter type for event handler. Type {entryType.FullName} is not assignable from the event type {type.FullName}. Method: {methodInfo.Name}({methodInfo.GetParameters().First().ParameterType.FullName})");
        }

        private void ProcessCommandHandlerMethod(MethodInfo methodInfo, Type methodArgType, BusRegistry registry)
        {
            ValidateReturnTypeIsTask(methodInfo);

            registry.AddCommandHandler(methodArgType, methodInfo);
        }


        private void ProcessQueryHanlderMethod(MethodInfo methodInfo, Type methodArgType, BusRegistry registry)
        {
            ValidateReturnTypeIsTaskOf(methodInfo, methodArgType.BaseType.GetGenericArguments()[0]);

            registry.AddQueryHandler(methodArgType, methodInfo);
        }

        private void ValidateReturnTypeIsTask(MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType == typeof(Task))
                return;

            throw new Exception(
                $"Invalid return type for potential Event or Command handler. Type {methodInfo.DeclaringType.FullName}. Method: {methodInfo.ReturnType.FullName} {methodInfo.Name}({methodInfo.GetParameters().First().ParameterType.FullName})");
        }

        private void ProcessEventHanlderMethod(MethodInfo methodInfo, Type methodArgType, BusRegistry registry)
        {
            ValidateReturnTypeIsTask(methodInfo);

            registry.AddEventSubscriber(methodArgType, methodInfo);
        }

        private void ValidateReturnTypeIsTaskOf(MethodInfo methodInfo, Type taskGenericParameter)
        {
            if (!methodInfo.ReturnType.IsGenericType || typeof(Task<>).IsAssignableToGenericType(methodInfo.ReturnType))
                throw new Exception($"Return type ({methodInfo.ReturnType.FullName}) type is not of Task<>");

            if (methodInfo.ReturnType.GetGenericArguments().Length == 0 ||
                methodInfo.ReturnType.GetGenericArguments()[0] != taskGenericParameter)
                throw new Exception(
                    $"Invalid return type for potential Query handler. Type {methodInfo.DeclaringType.FullName}. Method: {methodInfo.ReturnType.FullName} {methodInfo.Name}({methodInfo.GetParameters().First().ParameterType.FullName})");
        }
    }
}