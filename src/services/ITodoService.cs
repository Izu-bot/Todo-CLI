using todo.ErrorManagement;
using todo.src.model;

namespace todo.src.services;

public interface ITodoService
{
    Task<List<Todo>> GetAllAsync();
    Task<List<Todo>> GetTitleAsync(string title);
    Task<Todo> GetIdAsync(int id);
    Task AddTodoAsync(Todo todo);
    Task UpdateTodoAsync(Todo todo);
    Task DeleteTodoAsync(int id);
}
