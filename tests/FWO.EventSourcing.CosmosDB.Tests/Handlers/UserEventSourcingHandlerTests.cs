using FWO.EventSourcing.CosmosDB.Tests.TestAggregates;
using FWO.EventSourcing.CosmosDB.Tests.TestHelpers;
using Xunit;

namespace FWO.EventSourcing.CosmosDB.Tests.Handlers
{
    public class UserEventSourcingHandlerTests : IDisposable
    {
        private UserEventSourcingHandlerBuilder _builder;

        public UserEventSourcingHandlerTests()
        {
            _builder = new UserEventSourcingHandlerBuilder();
        }

        [Fact]
        public async Task Expect_SaveAsync_With_New_Aggregate_Succeeds()
        {
            var sut = _builder.Build();
            var aggregateId = default(Guid).ToString();
            var aggregate = new UserAggregate(aggregateId, "John", "Doe");
            aggregate.EditUser("Jack", "Daniels");

            await sut.SaveAsync(aggregate);

            var test = await sut.LoadAggregateByIdAsync(aggregateId);
            test.DeleteUser();
            await sut.SaveAsync(test);
        }

        public void Dispose()
        {
            _builder.Dispose();
        }
    }
}