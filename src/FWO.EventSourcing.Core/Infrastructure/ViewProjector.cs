using FWO.EventSourcing.Core.Domain;

namespace FWO.EventSourcing.Core.Infrastructure
{
    public abstract class ViewProjector : IViewProjector
    {
        public T ProjectView<T>(AggregateRoot aggregate) where T : new()
        {
            var events = aggregate.GetAppliedChanges();
            var view = new T();
            foreach (var @event in events)
            {
                var method = GetType().GetMethod("Project", new Type[]
                            {
                                @event.GetType(),
                                typeof(T)
                            });
                if (method == null)
                    throw new ArgumentException(nameof(method), $"The Project method was not found for {@event.GetType().Name}");

                method.Invoke(this, new object[] { @event, view });
            }

            return view;
        }
    }
}