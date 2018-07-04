# Marcus.Bus

**Highly opinionated**, asynchronous, in-memory, event bus for aps.net core with support for CQRS and EventSourcing.

Extracted from code base of a production system which runs on it since 2016.

## Design principles

Marcus.Bus has been designed to be light-weight and concise. The goal was to introduce CQRS and EventSourcing in ASP.net projects without relying on external messaging system.

Some of the invariants:
* each command has exactly one handler
* each event can have zero, one or more handlers 
* each query has exactly one handler
* each execution path/request can only start with command or query (you can't simply publish event)
* each published event has either source command or source event (each execution path can be traced from command/event store)
* bus uses .net async/await functionality to increase throughput


Marcus.Bus is splitted into two assemblies. Marcus.Bus.Abstractions which contains all base interfaces and base classes and Marcus.Bus which contains implementation and all required infrastructure code. Web project (ASP.net core) will reference Marcus.Bus and assemblies containing commands, events and handlers will reference Marcus.Bus.Abstractions.

Below is main bus interface. It provides means to dispatch commands, publish events and execute queries. This is the interface used in actions/endpoints.

```csharp
public interface IBus
{
    Task Handle<T>(T command) where T : Command;

    Task Publish<T>(T @event, Command sourceCommand) where T : Event;
    Task Publish<T>(T @event, Event sourceEvent) where T : Event;

    Task<T> Execute<T>(Query<T> query);
}
```
Each action in the system starts with a `Command`. Command handler can publish any number of events and each event can have zero, one or many handlers. Event handlers can also publish events.

Whenever developer publishes an event he has to provide the bus reference to source ``Command`` or source ``Event``. This way based on stored commands and events we can track what happened in the system (we can visualize each command execution as a tree which has a command at the root and events as children).

Dispatching command or publishing an event doesn't return any values. 

The only way to obtain values is to use queries. More about queries below. 

## Commands and Command Handlers 

Commands must inherit from the base ``Command`` type. See below list of properties from the base type. 
|Property                            |Description                         |
|------------------------------------|------------------------------------|
|`Guid CommandId`|Unique command id.|
|`string RequestId`|String identifier of the request in which command has been dispatched.|
|`string SessionId`|String identifier of the session. We generate unique session id every time user logs into the system.|
|`Guid TenantId`|Tenant identifier. Useful when command replay logic should work on tenant level.|
|`Guid PublishedBy`|User identifier (user which initiated command).|
|`DateTime PublishedUTC`|UTC date and time.|

### Commands validation

Bus will validate commands before executing command handlers. I found it significantly reduces chance for bugs (usually commands get corrupted by invalid data coming from the ui).

``Command`` base type does some basic validations but you can add additional validation by overriding ``void ValidateCommand()``.

Marcus.Bus is using [Marcus.Validation](https://github.com/belczyk/Marcus.Validation) library. ``Command`` is inheriting from ``ObjectWithValidation`` so developer has access to all Marcus.Validation methods inside ``ValidateCommand`` method.

See example below:
```csharp 
protected override void ValidateCommand()
{
    NotDefault(Id,nameof(Id));
    NotDefault(AssetId, nameof(AssetId));
    NotNullOrEmpty(DamageDescription, nameof(DamageDescription));
    NotDefault(StatusId, nameof(StatusId));
    AreValid(ExternalRepairs, nameof(ExternalRepairs));
    AreValid(InternalRepairs, nameof(InternalRepairs));
}
```

Also if your validation requires dependencies you can implement external validator for a command. If validator is registered in dependency injection container, the bus will find it and execute validation before handling command. 

Validator must implement ``ICommandValidator<in T> where T : Command`` interface, however it's much more handy to inherit from ``CommandValidator<T> where T : Command``, you'll get access to all Marcus.Validation methods.

### Command handler 
Command handlers must be in a type implementing marker interface ``IHandler``. 
There can be only one handler for a command and Bus will validate configuration and enforce this invariant at startup.

Command handler must be a public method returning ``Task`` and having one argument, a command.
See below example:
```csharp
public class RepairHandler : IHandler
{
    // ...
    public RepairHandler(IBus bus, IRepairRepository repairRepository)
    {
        // ...
    }

    public async Task Handle(CreateRepairFromRequestCommand command)
    {
        // ...
    }

    // ...
}
```

## Events and Event Handlers 

## Tracking 


## Metrics
