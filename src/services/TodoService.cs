using todo.ErrorManagement;
using todo.src.data.repository;
using todo.src.model;

namespace todo.src.services;

public class TodoService(ITodoRepository repository) : ITodoService
{
    private readonly ITodoRepository _repository = repository;

    public OperationsError AddTodo(Todo todo)
    {
        if (todo == null) return OperationsError.InvalidEntry;

        _repository.AddTodo(todo);
        return OperationsError.Success;
    }

    public OperationsError DeleteTodo(int id)
    {
        var todo = _repository.GetId(id);

        if (todo == null) return OperationsError.NotFound;

        _repository.DeleteTodo(todo);
        return OperationsError.Success;
    }

    public IQueryable<Todo> GetAll() => _repository.GetAll();

    public (OperationsError, Todo?) GetId(int id)
    {
        if (id <= 0) return (OperationsError.InvalidEntry, null);

        var todo = _repository.GetId(id);
        
        if(todo == null) return (OperationsError.NotFound, null);

        return (OperationsError.Success, todo);
    }

    public (OperationsError, IQueryable<Todo>) GetTitle(string title)
    {
        // Retorna uma coleção vazia
        if (String.IsNullOrWhiteSpace(title)) return (OperationsError.InvalidEntry, Enumerable.Empty<Todo>().AsQueryable());

        var todo = _repository.GetTitle(title);

        return todo.Any() ? (OperationsError.Success, todo) : (OperationsError.NotFound, Enumerable.Empty<Todo>().AsQueryable());
    }

    public OperationsError UpdateTodo(Todo todo)
    {
        if (todo == null) return OperationsError.InvalidEntry;

        var existingTodo = _repository.GetId(todo.Id);
        if (existingTodo == null) return OperationsError.NotFound;

        _repository.UpdateTodo(todo);

        return OperationsError.Success;
    }
}
