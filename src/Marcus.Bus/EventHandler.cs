using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Marcus.Bus
{
    public class EventHandler
    {
        public EventHandler(Type eventType)
        {
            EventType = eventType;
        }

        public Type EventType { get; }
        private IList<Tuple<Type, MethodInfo>> Subscribers { get; } = new List<Tuple<Type, MethodInfo>>();

        public void AddSubscriber(Type handlerType, MethodInfo methodInfo)
        {
            Subscribers.Add(new Tuple<Type, MethodInfo>(handlerType, methodInfo));
        }

        public async Task Publish(Event @event, IServiceProvider serviceProvider, IBus bus)
        {
            foreach (var subscriber in Subscribers)
                try
                {
                    var typeInstance = serviceProvider.GetService(subscriber.Item1);
                    var res = subscriber.Item2.Invoke(typeInstance, new object[] {@event});
                    ((Task) res).Wait();
                }
                catch (TargetInvocationException ex)
                {
                    await bus.Publish(new EventProcessingFailed(ex.InnerException ?? ex, @event), @event);
                    throw;
                }
                catch (Exception ex)
                {
                    await bus.Publish(new EventProcessingFailed(ex, @event), @event);
                    throw;
                }
        }
    }
}