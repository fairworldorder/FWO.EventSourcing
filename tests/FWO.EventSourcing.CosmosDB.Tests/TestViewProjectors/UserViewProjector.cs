using FWO.EventSourcing.Core.Infrastructure;
using FWO.EventSourcing.CosmosDB.Tests.TestAggregates;
using FWO.EventSourcing.CosmosDB.Tests.TestEvents;
using Xunit;

namespace FWO.EventSourcing.CosmosDB.Tests.TestViewProjectors
{
    public class UserViewProjectorTests
    {
        [Fact]
        public async Task Test()
        {
            var sut = new UserViewProjector();

            var aggregateId = default(Guid).ToString();
            var aggregate = new UserAggregate(aggregateId, "John", "Doe");
            aggregate.EditUser("Jack", "Daniels");
            aggregate.EditUser("Jack", "Sparrow");
            aggregate.CommitChanges();

            var view = sut.ProjectView<UserView>(aggregate);
        }
    }

    public class UserViewProjector : ViewProjector
    {
        public void Project(StartedEvent @event, UserView view)
        {
            view.FirstName = @event.FirstName;
            view.LastName = @event.LastName;
            view.DateOfBirth = @event.DateOfBirth;
            view.IsActive = true;
        }

        public void Project(ModifiedEvent @event, UserView view)
        {
            view.FirstName = @event.FirstName;
            view.LastName = @event.LastName;
        }

        public void Project(DeletedEvent @event, UserView view)
        {
            view.IsActive = false;
        }
    }

    public class UserView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
    }
}