using System;
using System.Collections.Generic;
using System.Reflection;
using TestAssembly1;
using TestAssembly1.InvalidHandlers;
using Xunit;

namespace Marcus.Bus.Tests
{
    public class BusRegistryBuilderTests
    {
        private readonly Func<Assembly, bool> AssemblySelector = x => x.FullName.Contains("TestAssembly1");

        private BusRegistryBuilder GivenBulderForValidHandlers()
        {
            return new BusRegistryBuilder(AssemblySelector, x => x.Namespace.Contains("ValidHandlers"));
        }

        [Fact]
        public void Bootstrap_finds_command_event_and_query_handlers()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.True(registry.HasCommandHanlder(typeof(Command1)));
            Assert.True(registry.HasCommandHanlder(typeof(Command2)));
            Assert.True(registry.HasCommandHanlder(typeof(Command3)));
            Assert.True(registry.HasAnySubscribers<Command1ExecutedEvent>());
            Assert.True(registry.HasAnySubscribers<Command2ExecutedEvent>());
            Assert.True(registry.HasAnySubscribers<Command3ExecutedEvent>());
            Assert.True(registry.HasQueryHanlder(typeof(Query1Query)));
            Assert.True(registry.HasValidator(new Command1()));
        }

        [Fact]
        public void Bootstrap_throws_when_command_handler_has_wrong_return_type()
        {
            // given
            var builder = new BusRegistryBuilder(typeof(InvalidCommandHandler3));

            // when/then
            Assert.Throws<Exception>(() => builder.Bootstrap());
        }

        [Fact]
        public void Bootstrap_throws_when_multi_command_handler_has_wrong_type_as_an_argument()
        {
            // given
            var builder = new BusRegistryBuilder(typeof(InvalidMultiCommandHandler));

            // when/then
            Assert.Throws<Exception>(() => builder.Bootstrap());
        }

        [Fact]
        public void Bootstrap_throws_when_multi_event_handler_has_wrong_type_as_an_argument()
        {
            // given
            var builder = new BusRegistryBuilder(typeof(InvalidMultiEventHandler));

            // when/then
            Assert.Throws<Exception>(() => builder.Bootstrap());
        }

        [Fact]
        public void Bootstrap_throws_when_query_handler_has_not_generic_task_as_return_type()
        {
            // given
            var builder = new BusRegistryBuilder(typeof(InvalidQueryHandler2));

            // when/then
            Assert.Throws<Exception>(() => builder.Bootstrap());
        }

        [Fact]
        public void Bootstrap_throws_when_query_handler_has_not_task_as_return_type()
        {
            // given
            var builder = new BusRegistryBuilder(typeof(InvalidQueryHandler3));

            // when/then
            Assert.Throws<Exception>(() => builder.Bootstrap());
        }


        [Fact]
        public void Bootstrap_throws_when_query_handler_returns_task_of_wrong_type()
        {
            // given
            var builder = new BusRegistryBuilder(typeof(InvalidQueryHandler1));

            // when/then
            Assert.Throws<Exception>(() => builder.Bootstrap());
        }

        [Fact]
        public void Bootstrap_throws_when_two_handlers_for_same_command_registered()
        {
            // given
            var builder = new BusRegistryBuilder(typeof(InvalidCommandHandler1), typeof(InvalidCommandHandler2));

            // when/then
            Assert.Throws<InvalidOperationException>(() => builder.Bootstrap());
        }


        [Fact]
        public void Bootstrap_throws_when_two_handlers_for_same_query_registered()
        {
            // given
            var builder = new BusRegistryBuilder(typeof(InvalidQueryHandler4a), typeof(InvalidQueryHandler4b));

            // when/then
            Assert.Throws<InvalidOperationException>(() => builder.Bootstrap());
        }

        [Fact]
        public void GetCommandHandler_returns_handler_when_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.NotNull(registry.GetCommandHandler(typeof(Command1)));
            Assert.NotNull(registry.GetCommandHandler(typeof(Command2)));
            Assert.NotNull(registry.GetCommandHandler(typeof(Command3)));
        }

        [Fact]
        public void GetCommandHandler_throws_if_command_handler_not_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.Throws<KeyNotFoundException>(() => registry.GetCommandHandler(typeof(CommandWithoutHandler)));
        }

        [Fact]
        public void GetEventdHandler_throws_if_event_handler_not_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.Throws<KeyNotFoundException>(() => registry.GetEventHandler(typeof(EventWithoutHanlder1)));
        }

        [Fact]
        public void GetEventHandler_returns_handler_when_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then

            Assert.NotNull(registry.GetEventHandler(typeof(Command1ExecutedEvent)));
            Assert.NotNull(registry.GetEventHandler(typeof(Command2ExecutedEvent)));
            Assert.NotNull(registry.GetEventHandler(typeof(Command3ExecutedEvent)));
        }

        [Fact]
        public void GetQueryHandler_returns_handler_when_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then

            Assert.NotNull(registry.GetQueryHandler(typeof(Query1Query)));
        }

        [Fact]
        public void GetQueryHandler_throws_if_query_handler_not_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.Throws<KeyNotFoundException>(() => registry.GetQueryHandler(typeof(QueryWithoutHandler)));
        }

        [Fact]
        public void GetValidator_returns_validator_when_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.NotNull(registry.GetValidator(new Command1()));
        }

        [Fact]
        public void GetValidator_throws_if_validator_not_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.Throws<KeyNotFoundException>(() => registry.GetValidator(new Command2()));
        }

        [Fact]
        public void HasAnySubscribers_returns_false_if_no_handler_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.False(registry.HasAnySubscribers<EventWithoutHanlder1>());
        }


        [Fact]
        public void HasAnySubscribers_returns_true_if_any_event_handler_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.True(registry.HasAnySubscribers<Command1ExecutedEvent>());
        }

        [Fact]
        public void HasCommandHanlder_returns_false_if_handler_not_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.False(registry.HasCommandHanlder(typeof(CommandWithoutHandler)));
        }

        [Fact]
        public void HasCommandHanlder_returns_true_if_handler_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.True(registry.HasCommandHanlder(typeof(Command1)));
        }

        [Fact]
        public void HasQueryHanlder_returns_false_if_handler_not_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.False(registry.HasQueryHanlder(typeof(QueryWithoutHandler)));
        }

        [Fact]
        public void HasQueryHanlder_returns_true_if_handler_available()
        {
            // given
            var builder = GivenBulderForValidHandlers();

            // when
            var registry = builder.Bootstrap();

            // then
            Assert.True(registry.HasQueryHanlder(typeof(Query1Query)));
        }
    }
}