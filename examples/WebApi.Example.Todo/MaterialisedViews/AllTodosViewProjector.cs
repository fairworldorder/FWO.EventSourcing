using FWO.EventSourcing.Core.Infrastructure;
using WebApi.Example.Todo.Events;
using WebApi.Example.Todo.Models;

namespace WebApi.Example.Todo.MaterialisedViews
{
    public class AllTodosViewProjector : ViewProjector
    {
        public void Project(TodoCreated @event, AllTodosView view)
        {
            view.Todos.Add(new TodoModel
            {
                Id = @event.Id,
                Active = @event.Active,
                Data = @event.Data
            });
        }

        public void Project(TodoUpdated @event, AllTodosView view)
        {
            view.Todos.Where(x => x.Id == @event.Id).ToList().ForEach(x =>
            {
                x.Data = @event.Data;
                x.Active = @event.Active;
            });
        }

        public void Project(TodoDeleted @event, AllTodosView view)
        {
            view.Todos.RemoveAll(x => x.Id == @event.Id);
        }
    }
}
