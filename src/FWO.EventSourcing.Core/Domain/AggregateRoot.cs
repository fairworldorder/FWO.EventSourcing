using FWO.EventSourcing.Core.Events;

namespace FWO.EventSourcing.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected string _id;
        private readonly List<BaseEvent> _changes = new();

        public string Id { get => _id; }

        public int Version { get; private set; } = -1;

        public IEnumerable<BaseEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void CommitChanges()
        {
            Version += _changes.Count();
            _changes.Clear();
        }

        private void ApplyChange(BaseEvent @event, bool isNew)
        {
            var method = GetType().GetMethod("Apply", new Type[]
            {
                @event.GetType()
            });
            if (method == null)
                throw new ArgumentException(nameof(method), $"The apply method was not found in the aggregate for {@event.GetType().Name}");

            method.Invoke(this, new object[] { @event });
            if (isNew)
                _changes.Add(@event);
            else
                Version++;
        }

        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChange(@event, true);
        }

        public void LoadEvents(IEnumerable<BaseEvent> events, string aggregateId)
        {
            _id = aggregateId;
            Version = -1;
            foreach (var @event in events)
            {
                ApplyChange(@event, false);
            }
        }
    }
}