using FWO.EventSourcing.Core.Domain;
using FWO.EventSourcing.CosmosDB.Tests.TestEvents;

namespace FWO.EventSourcing.CosmosDB.Tests.TestAggregates
{
    public class UserAggregate : AggregateRoot
    {
        private string _firstName;
        private string _lastName;
        private bool _active;

        public UserAggregate()
        {
        }

        public UserAggregate(string aggregateId, string firstName, string lastName)
        {
            _id = aggregateId;

            RaiseEvent(new StartedEvent
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                //Type = nameof(StartedEvent)
            });
        }

        public void EditUser(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                throw new ArgumentException("Invalid argument");

            if (!_active)
                throw new InvalidOperationException("Inactive users cannot be modified");

            RaiseEvent(new ModifiedEvent
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                //Type = nameof(ModifiedEvent)
            });
        }

        public void DeleteUser()
        {
            if (!_active)
                throw new InvalidOperationException("User is already deleted");

            RaiseEvent(new DeletedEvent
            {
                Id = Guid.NewGuid().ToString(),
                //Type = nameof(DeletedEvent)
            });
        }

        public void Apply(StartedEvent @event)
        {
            _firstName = @event.FirstName;
            _lastName = @event.LastName;
            _active = true;
        }

        public void Apply(ModifiedEvent @event)
        {
            _firstName = @event.FirstName;
            _lastName = @event.LastName;
        }

        public void Apply(DeletedEvent @event)
        {
            _active = false;
        }
    }
}