using todo.src.model;

namespace todo.src.data.repository;

public interface ITodoRepository
{
    IQueryable<Todo> GetAll();
    IQueryable<Todo> GetTitle(string title);
    Task<Todo?> GetIdAsync(int id);
    Task AddTodoAsync(Todo todo);
    Task UpdateTodoAsync(Todo todo);
    Task DeleteTodoAsync(Todo todo);
}
