using FWO.EventSourcing.Core.Events;

namespace FWO.EventSourcing.Core.Infrastructure
{
    public interface IEventResolver
    {
        /// <summary>
        /// Resolves a list of events derived from <see cref="BaseEvent"/> from <see cref="EventModel.EventData"/>.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        List<BaseEvent> ResolveEvents(List<EventModel> eventModels);
    }
}