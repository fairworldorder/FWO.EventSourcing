using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.CosmosDB.Tests.Data;
using FWO.EventSourcing.CosmosDB.Tests.Events;
using Xunit;

namespace FWO.EventSourcing.CosmosDB.Tests.Infrastructure
{
    public class CosmosEventStoreRepositoryTests
    {
        private CosmosEventStoreRepositoryBuilder _builder;

        public CosmosEventStoreRepositoryTests()
        {
            _builder = new CosmosEventStoreRepositoryBuilder();
        }

        [Fact]
        public async Task Expect_SaveAsync_Succeeds_When_EventModel_Specified()
        {
            var sut = _builder.Build();
            var aggregateId = Guid.NewGuid().ToString();
            var eventModel = new EventModel
            {
                Id = Guid.NewGuid().ToString(),
                AggregateId = aggregateId,
                AggregateType = "TestAggregate",
                EventType = nameof(StartedEvent),
                EventData = new StartedEvent
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Id = Guid.NewGuid().ToString(),
                    Type = nameof(StartedEvent),
                    Version = 0
                }
            };

            await sut.SaveAsync(eventModel);
        }

        [Fact]
        public async Task Expect_LoadByAggregateIdAsync()
        {
            var sut = _builder.Build();
            var aggregateId = Guid.NewGuid().ToString();

            var startedEvent = new StartedEvent
            {
                FirstName = "John",
                LastName = "Doe",
                Id = Guid.NewGuid().ToString(),
                Type = nameof(StartedEvent),
                Version = 0,
            };
            await sut.SaveAsync(new EventModel
            {
                Id = $"{aggregateId}_0",
                AggregateId = aggregateId,
                AggregateType = "TestAggregate",
                EventType = nameof(StartedEvent),
                EventData = startedEvent,
                Timestamp = DateTime.UtcNow,
                Version = 0
            });

            var modifiedEvent = new ModifiedEvent
            {
                FirstName = "Joe",
                LastName = "Schmoe",
                Id = Guid.NewGuid().ToString(),
                Type = nameof(ModifiedEvent),
                Version = 1
            };
            await sut.SaveAsync(new EventModel
            {
                Id = $"{aggregateId}_1",
                AggregateId = aggregateId,
                AggregateType = "TestAggregate",
                EventType = nameof(ModifiedEvent),
                EventData = modifiedEvent,
                Version = 1
            });

            var deletedEvent = new DeletedEvent
            {
                Id = Guid.NewGuid().ToString(),
                Type = nameof(DeletedEvent),
                Version = 2
            };
            await sut.SaveAsync(new EventModel
            {
                Id = $"{aggregateId}_2",
                AggregateId = aggregateId,
                AggregateType = "TestAggregate",
                EventType = nameof(DeletedEvent),
                EventData = deletedEvent,
                 Version = 2
            });


            var events = await sut.LoadByAggregateIdAsync(aggregateId);

            Assert.Collection(events,
                ev =>
                {
                    Assert.Equal(nameof(StartedEvent), ev.EventType);
                    Assert.Equal(0, ev.Version);
                    Assert.Equal(startedEvent.FirstName, ((StartedEvent)ev.EventData).FirstName);
                    Assert.Equal(startedEvent.LastName, ((StartedEvent)ev.EventData).LastName);
                },
                ev =>
                {
                    Assert.Equal(nameof(StartedEvent), ev.EventType);
                    Assert.Equal(1, ev.Version);
                    Assert.Equal(modifiedEvent.FirstName, ((StartedEvent)ev.EventData).FirstName);
                    Assert.Equal(modifiedEvent.LastName, ((StartedEvent)ev.EventData).LastName);
                },
                ev =>
                {
                    Assert.Equal(nameof(DeletedEvent), ev.EventType);
                    Assert.Equal(2, ev.Version);
                });
        }
    }
}