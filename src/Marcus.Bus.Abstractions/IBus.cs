using System.Threading.Tasks;

namespace Marcus.Bus
{
    public interface IBus
    {
        Task Publish<T>(T @event, Command sourceCommand) where T : Event;
        Task Publish<T>(T @event, Event sourceEvent) where T : Event;
        Task Handle<T>(T command) where T : Command;
        Task<T> Execute<T>(Query<T> query);
    }
}