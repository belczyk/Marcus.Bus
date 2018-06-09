using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marcus.Bus
{
    public interface ICommandsEventsStore
    {
        Task PersistCommand(Command command);
        Task PersistEvent(Event @event);
        Task PersistEvents(IList<Event> events);
        Task<IList<Command>> GetCommandsByTenantId(Guid tenantId);
    }
}