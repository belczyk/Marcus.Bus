using System;
using System.Threading.Tasks;
using Marcus.Bus.Abstractions;
using TestAssembly1;
using Xunit;

namespace Marcus.Bus.Tests
{
    public class BusTests
    {
        private readonly IServiceProvider serviceProvider = new FakeServiceProvider();


        private IBus GivenBus()
        {
            Bus.Init(a => a.FullName.Contains("TestAssembly1"), t => t.Namespace.Contains("ValidHandlers"));

            return (IBus) serviceProvider.GetService(typeof(IBus));
        }


        [Fact]
        public async Task Execute_returns_query_result_when_query_handler_available()
        {
            // given 
            var bus = GivenBus();

            //when 
            var result = await bus.Execute(new Query1Query(10));

            //then
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task Execute_throws_if_handler_not_available()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await Assert.ThrowsAsync<MissingQueryHandlerException>(() => bus.Execute(new QueryWithoutHandler()));
        }


        [Fact]
        public async Task Execute_throws_if_query_is_null()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await Assert.ThrowsAsync<QueryCanNotBeNullException>(() => bus.Execute((Query1Query) null));
        }


        [Fact]
        public async Task Handle_throws_if_command_is_null()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await Assert.ThrowsAsync<CommandCannotBeNullException>(() => bus.Handle((Command) null));
        }

        [Fact]
        public async Task Handle_throws_if_handler_not_available()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await Assert.ThrowsAsync<MissingCommandHandlerException>(() => bus.Handle(new CommandWithoutHandler()));
        }

        [Fact]
        public async Task Handle_throws_when_command_with_validation_is_invalid()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await Assert.ThrowsAsync<InvalidCommandException>(() => bus.Handle(new CommandWithValidation(false)));
        }

        [Fact]
        public async Task Handles_command_if_command_handler_available()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await bus.Handle(new Command1());
        }

        [Fact]
        public async Task Handles_command_which_has_been_executed_in_past()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await bus.Handle(new Command1 {PublishedUTC = new DateTime(2010, 1, 1)});
        }

        [Fact]
        public async Task Handles_valid_command_with_validation()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await bus.Handle(new CommandWithValidation(true));
        }


        [Fact]
        public async Task Publish_throws_if_event_is_null_and_source_is_command()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await Assert.ThrowsAsync<EventCannotBeNullException>(() => bus.Publish((Event) null, new Command1()));
        }

        [Fact]
        public async Task Publish_throws_if_event_is_null_and_source_is_event()
        {
            // given 
            var bus = GivenBus();

            //when/then
            await Assert.ThrowsAsync<EventCannotBeNullException>(() =>
                bus.Publish((Event) null, new EventWithEventSource()));
        }

        [Fact]
        public async Task Publishes_event_when_handler_exists_and_source_is_a_command()
        {
            // given 
            var bus = GivenBus();

            //when
            await bus.Publish(new Command1ExecutedEvent(), new Command1());
        }


        [Fact]
        public async Task Publishes_event_when_handler_exists_and_source_is_an_event()
        {
            // given 
            var bus = GivenBus();

            //when
            await bus.Publish(new EventWithEventSource(), new Command1ExecutedEvent());
        }
    }
}