using FWO.EventSourcing.Core.Events;
using FWO.EventSourcing.CosmosDB.Tests.TestEvents;
using FWO.EventSourcing.CosmosDB.Tests.TestHelpers;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FWO.EventSourcing.CosmosDB.Tests.Infrastructure
{
    public class CosmosEventStoreRepositoryTests : IDisposable
    {
        private CosmosEventStoreRepositoryBuilder _builder;
        private EventModelListBuilder _eventModelListBuilder;

        public CosmosEventStoreRepositoryTests()
        {
            _builder = new CosmosEventStoreRepositoryBuilder();
            _eventModelListBuilder = new EventModelListBuilder();
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
                    DateOfBirth = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    //Type = nameof(StartedEvent),
                    //Version = 0
                }
            };

            await sut.SaveAsync(eventModel);
        }

        [Fact]
        public async Task Expect_LoadByAggregateIdAsync()
        {
            var sut = _builder.Build();
            var eventModels = _eventModelListBuilder.SetDefault().Build();

            foreach (var eventModel in eventModels)
            {
                await sut.SaveAsync(eventModel);
            }

            string aggregateId = eventModels[0].AggregateId;

            var events = await sut.LoadByAggregateIdAsync(aggregateId);

            Assert.Collection(events,
                ev =>
                {
                    var @event = ((JObject)ev.EventData).ToObject<StartedEvent>();
                    //Assert.Equal(nameof(StartedEvent), @event.Type);
                    //Assert.Equal(0, @event.Version);
                    Assert.Equal(eventModels[0].EventData.FirstName, @event.FirstName);
                    Assert.Equal(eventModels[0].EventData.LastName, @event.LastName);
                },
                ev =>
                {
                    var @event = ((JObject)ev.EventData).ToObject<ModifiedEvent>();
                    //Assert.Equal(nameof(ModifiedEvent), @event.Type);
                    //Assert.Equal(1, @event.Version);
                    Assert.Equal(eventModels[1].EventData.FirstName, @event.FirstName);
                    Assert.Equal(eventModels[1].EventData.LastName, @event.LastName);
                },
                ev =>
                {
                    var @event = ((JObject)ev.EventData).ToObject<DeletedEvent>();
                    //Assert.Equal(nameof(DeletedEvent), @event.Type);
                    //Assert.Equal(2, @event.Version);
                });
        }

        public void Dispose()
        {
            _builder.Dispose();
        }
    }
}