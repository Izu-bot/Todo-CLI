using todo.ErrorManagement;
using todo.src.data.repository;
using todo.src.model;

namespace todo.src.services;

public class TodoService(ITodoRepository repository) : ITodoService
{
    private readonly ITodoRepository _repository = repository;

    public OperationsStatus AddTodo(Todo todo)
    {
        if (todo == null) return OperationsStatus.InvalidEntry;

        _repository.AddTodo(todo);
        return OperationsStatus.Success;
    }

    public OperationsStatus DeleteTodo(int id)
    {
        var todo = _repository.GetId(id);

        if (todo == null) return OperationsStatus.NotFound;

        _repository.DeleteTodo(todo);
        return OperationsStatus.Success;
    }

    public IQueryable<Todo> GetAll() => _repository.GetAll();

    public (OperationsStatus, Todo?) GetId(int id)
    {
        if (id <= 0) return (OperationsStatus.InvalidEntry, null);

        var todo = _repository.GetId(id);
        
        if(todo == null) return (OperationsStatus.NotFound, null);

        return (OperationsStatus.Success, todo);
    }

    public (OperationsStatus, IQueryable<Todo>) GetTitle(string title)
    {
        // Retorna uma coleção vazia
        if (String.IsNullOrWhiteSpace(title)) return (OperationsStatus.InvalidEntry, Enumerable.Empty<Todo>().AsQueryable());

        var todo = _repository.GetTitle(title);

        return todo.Any() ? (OperationsStatus.Success, todo) : (OperationsStatus.NotFound, Enumerable.Empty<Todo>().AsQueryable());
    }

    public OperationsStatus UpdateTodo(Todo todo)
    {
        if (todo == null) return OperationsStatus.InvalidEntry;

        var existingTodo = _repository.GetId(todo.Id);
        if (existingTodo == null) return OperationsStatus.NotFound;

        _repository.UpdateTodo(todo);

        return OperationsStatus.Success;
    }
}
