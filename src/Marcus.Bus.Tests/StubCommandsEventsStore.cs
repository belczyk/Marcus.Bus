using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marcus.Bus.Tests
{
    public class StubCommandsEventsStore : ICommandsEventsStore
    {
        public Task PersistCommand(Command command)
        {
            return Task.CompletedTask;
        }

        public Task PersistEvent(Event @event)
        {
            return Task.CompletedTask;
        }

        public Task PersistEvents(IList<Event> events)
        {
            return Task.CompletedTask;
        }

        public Task<IList<Command>> GetCommandsByTenantId(Guid tenantId)
        {
            return Task.FromResult((IList<Command>) new List<Command>());
        }
    }
}