using FWO.EventSourcing.Core.Events;

namespace WebApi.Example.Todo.Events
{
    public class TodoUpdated : BaseEvent
    {
        public DateTime UpdatedDate { get; set; }
        public string Data { get; set; }
        public bool Active { get; set; }
    }
}