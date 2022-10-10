using FWO.EventSourcing.Core.Domain;
using WebApi.Example.Todo.Events;

namespace WebApi.Example.Todo.Aggregates
{
    public class TodoAggregate : AggregateRoot
    {
        private List<string> _todoIds = new List<string>();

        public TodoAggregate()
        {
        }

        public TodoAggregate(string aggregateId, string todoData, DateTime todoCreatedDate)
        {
            _id = aggregateId;
            RaiseEvent(new TodoCreated
            {
                Id = Guid.NewGuid().ToString(),
                Created = todoCreatedDate,
                Active = true,
                Data = todoData
            });
        }

        public void CreateTodo(string todoData, DateTime todoCreatedDate)
        {
            var todoId = Guid.NewGuid().ToString();
            RaiseEvent(new TodoCreated
            {
                Id = todoId,
                Created = todoCreatedDate,
                Active = true,
                Data = todoData
            });
        }

        public void UpdateTodo(string todoId, string todoData, bool todoActive, DateTime updateDate)
        {
            if(!_todoIds.Contains(todoId))
                throw new InvalidOperationException("Invalid Todo Id.");

            RaiseEvent(new TodoUpdated
            {
                Id = todoId,
                UpdatedDate = updateDate,
                Active = todoActive,
                Data = todoData
            });
        }

        public void DeleteTodo(string todoId)
        {
            if (!_todoIds.Contains(todoId))
                throw new InvalidOperationException("Invalid Todo Id.");

            RaiseEvent(new TodoDeleted
            {
                Id= todoId
            });
        }

        public void Apply(TodoCreated @event)
        {
            _todoIds.Add(@event.Id);
        }

        public void Apply(TodoUpdated @event)
        {
        }

        public void Apply(TodoDeleted @event)
        {
            _todoIds.Remove(@event.Id);
        }
    }
}
