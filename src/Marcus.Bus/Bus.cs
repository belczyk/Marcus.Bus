using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Marcus.Bus.Abstractions;

namespace Marcus.Bus
{
    [DebuggerStepThrough]
    public class Bus : IBus
    {
        private readonly IList<Event> Events = new List<Event>(8);

        public Bus(IServiceProvider serviceProvider
            , IBusSession sessionAccessor
            , ICommandsEventsStore commandsEventsStore
            , IMetricService metricService
            , ITime time)
        {
            ServiceProvider = serviceProvider;
            SessionAccessor = sessionAccessor;
            CommandsEventsStore = commandsEventsStore;
            MetricService = metricService;
            Time = time;
        }

        public static BusRegistry Registry { get; set; }
        private IServiceProvider ServiceProvider { get; }
        private IBusSession SessionAccessor { get; }
        private ICommandsEventsStore CommandsEventsStore { get; }
        public IMetricService MetricService { get; }
        public ITime Time { get; }

        public async Task Publish<T>(T @event, Event sourceEvent) where T : Event
        {
            if (@event == null)
                throw new EventCannotBeNullException(typeof(T).Name);

            @event.SourceEventId = sourceEvent.EventId;
            @event.SourceCommandId = sourceEvent.SourceCommandId;
            @event.PublishedBy = sourceEvent.PublishedBy;
            @event.RequestId = sourceEvent.RequestId;
            @event.SessionId = sourceEvent.SessionId;
            @event.TenantId = sourceEvent.TenantId;

            await PublishEvent(@event);
        }

        public async Task Publish<T>(T @event, Command sourceCommand) where T : Event
        {
            if (@event == null)
                throw new EventCannotBeNullException(typeof(T).Name);

            @event.SourceCommandId = sourceCommand.CommandId;
            @event.PublishedBy = sourceCommand.PublishedBy;
            @event.SessionId = sourceCommand.SessionId;
            @event.RequestId = sourceCommand.RequestId;
            @event.TenantId = sourceCommand.TenantId;

            await PublishEvent(@event);
        }

        public async Task<T> Execute<T>(Query<T> query)
        {
            if (query == null) throw new QueryCanNotBeNullException();

            var queryType = query.GetType();
            if (!Registry.HasQueryHanlder(queryType)) throw new MissingQueryHandlerException(queryType.Name);

            var metric = MetricService.StartForType(query, "QueryProcessing");
            var queryExecuter = Registry.GetQueryHandler(queryType);
            var res = await queryExecuter.Execute(query, ServiceProvider);
            metric.Stop();

            return res;
        }

        public async Task Handle<T>(T command) where T : Command
        {
            var commandHandlingMetric = MetricService.StartForType(command, "CommandHandling");
            try
            {
                if (command == null) throw new CommandCannotBeNullException(typeof(T).Name);

                if (command.PublishedUTC != default(DateTime))
                    Time.OverrideTime(command.PublishedUTC);


                SetCorrelationProperties(command);

                await ValidateCommand(command);

                var commandKey = command.GetType();

                if (!Registry.HasCommandHanlder(commandKey)) throw new MissingCommandHandlerException(commandKey.Name);

                try
                {
                    await CommandsEventsStore.PersistCommand(command);
                    var metric = MetricService.StartForType(command, "CommandProcessing");
                    await Registry.GetCommandHandler(commandKey).Handle(command, ServiceProvider);
                    metric.Stop();

                    await PersitEvents();
                }
                catch (TargetInvocationException ex)
                {
                    await Publish(new CommandExecutionFailed(ex.InnerException ?? ex, command), command);
                    throw;
                }
                catch (Exception ex)
                {
                    await Publish(new CommandExecutionFailed(ex, command), command);
                    throw;
                }
                finally
                {
                    await PersitEvents();
                }
            }
            finally
            {
                commandHandlingMetric.Stop();
            }
        }

        private async Task PersitEvents()
        {
            await CommandsEventsStore.PersistEvents(Events);
            Events.Clear();
        }

        private async Task ValidateCommand(Command command)
        {
            var result = command.Validate();
            if (result.IsNotValid)
                throw new InvalidCommandException(result);

            if (Registry.HasValidator(command))
            {
                var validator = Registry.GetValidator(command);
                result = await validator.Validate(command, ServiceProvider);
                if (result.IsNotValid) throw new InvalidCommandException(result);
            }
        }

        public static void Init(string namespacePrefix)
        {
            var busRegistryBuilder = new BusRegistryBuilder(a => a.GetName().Name.StartsWith(namespacePrefix));
            Registry = busRegistryBuilder.Bootstrap();
        }

        public static void Init(Func<Assembly, bool> assemblySelector, Func<Type, bool> typeSelector)
        {
            var busRegistryBuilder = new BusRegistryBuilder(assemblySelector, typeSelector);
            Registry = busRegistryBuilder.Bootstrap();
        }

        private async Task PublishEvent<T>(T @event) where T : Event
        {
            @event.PublishedUTC = SessionAccessor.Now;
            Events.Add(@event);
            var metric = MetricService.StartForType(@event, "EventProcessing");

            if (Registry.HasAnySubscribers<T>()) await ProcessEventHandlers(@event);

            metric.Stop();
        }

        private async Task ProcessEventHandlers<T>(T @event) where T : Event
        {
            var eventPublisher = Registry.GetEventHandler(typeof(T));
            await eventPublisher.Publish(@event, ServiceProvider, this);
        }

        protected void SetCorrelationProperties(Command command)
        {
            if (command.RequestId == null)
                command.RequestId = SessionAccessor.RequestId;
            if (command.SessionId == null)
                command.SessionId = SessionAccessor.SessionId;
            if (command.TenantId == default(Guid))
                command.TenantId = SessionAccessor.TenantId;
            if (command.PublishedBy == default(Guid))
                command.PublishedBy = SessionAccessor.UserId;
            if (command.PublishedUTC == default(DateTime))
                command.PublishedUTC = SessionAccessor.Now;
        }
    }
}