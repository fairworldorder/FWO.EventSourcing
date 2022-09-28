using FWO.EventSourcing.Core.Exceptions;
using FWO.EventSourcing.CosmosDB.Tests.TestHelpers;
using Xunit;

namespace FWO.EventSourcing.CosmosDB.Tests.Infrastructure
{
    public class EventStoreTests : IDisposable
    {
        private EventStoreBuilder _eventStoreBuilder;
        private EventListBuilder _eventListBuilder;

        public EventStoreTests()
        {
            _eventStoreBuilder = new();
            _eventListBuilder = new();
        }

        [Fact]
        public async Task Expect_SaveEventAsync_To_New_Aggregate_Succeeds()
        {
            // Arrange

            var sut = _eventStoreBuilder.Build();
            var aggregateId = default(Guid).ToString();
            var events = _eventListBuilder.SetDefault().Build();

            // Act

            var exception = await Record.ExceptionAsync(async () =>
            {
                await sut.SaveEventAsync(aggregateId, "TestAggregate", events, -1);
            });

            // Assert

            Assert.Null(exception);
        }

        [Fact]
        public async Task Expect_SaveEventAsync_To_Existing_Aggregate_Succeeds_When_Valid_Version_Specified()
        {
            // Arrange

            var sut = _eventStoreBuilder.Build();
            var aggregateId = default(Guid).ToString();
            var events = _eventListBuilder
                .AddStartedEvent("John", "Doe")
                .AddModifiedEvent("Joe", "Schmoe")
                .Build();

            await sut.SaveEventAsync(aggregateId, "TestAggregate", events, -1);

            var newEvents = _eventListBuilder.Clear().AddModifiedEvent("Jane", "Doe").Build();

            

            var exception = await Record.ExceptionAsync(async () =>
            {
                await sut.SaveEventAsync(aggregateId, "TestAggregate", newEvents, 1);
            });

            // Assert

            Assert.Null(exception);
        }

        [Fact]
        public async Task Expect_SaveEventAsync_To_Existing_Aggregate_Throws_ConcurrencyException_When_Invalid_Version_Specified()
        {
            // Arrange

            var sut = _eventStoreBuilder.Build();
            var aggregateId = default(Guid).ToString();
            var events = _eventListBuilder
                .AddStartedEvent("John", "Doe")
                .AddModifiedEvent("Joe", "Schmoe")
                .Build();

            await sut.SaveEventAsync(aggregateId, "TestAggregate", events, -1);

            var newEvents = _eventListBuilder.Clear().AddModifiedEvent("Jane", "Doe").Build();

            var exception = await Assert.ThrowsAsync<ConcurrencyException>(() => sut.SaveEventAsync(aggregateId, "TestAggregate", newEvents, 99));


            // Assert

            Assert.NotNull(exception);
        }


        public void Dispose()
        {
            _eventStoreBuilder.Dispose();
        }
    }
}