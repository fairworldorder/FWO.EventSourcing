using FWO.EventSourcing.Core.Events;

namespace WebApi.Example.Todo.Events
{
    public class TodoCreated : BaseEvent
    {
        public DateTime Created { get; set; }
        public string Data { get; set; }
        public bool Active { get; set; }
    }
}
