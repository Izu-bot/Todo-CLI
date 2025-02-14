using Microsoft.EntityFrameworkCore;
using todo.src.data.repository;
using todo.src.model;

namespace todo.src.services;

public class TodoService(ITodoRepository repository) : ITodoService
{
    private readonly ITodoRepository _repository = repository;

    public async Task AddTodoAsync(Todo todo)
    {
        await _repository.AddTodoAsync(todo);
    }

    public async Task DeleteTodoAsync(int id)
    {
        var usuario = await _repository.GetIdAsync(id);

        if (usuario == null)
        {
            throw new NullReferenceException("Todo not found");
        }
        else
        {
            await _repository.DeleteTodoAsync(usuario);
        }
    }

    public async Task<List<Todo>> GetAllAsync()
    {
        return await _repository.GetAll().ToListAsync();
    }

    public async Task<Todo> GetIdAsync(int id)
    {
        var usuario = await _repository.GetIdAsync(id);

        if (usuario != null)
        {
            return usuario;
        }
        else
        {
            return null!;
        }
    }

    public async Task<List<Todo>> GetTitleAsync(string title)
    {
        var todos = await _repository.GetTitle(title).ToListAsync();

        if (todos.Count == 0)
        {
            return null!;
        }
        else
        {
            return todos;
        }
    }

    public async Task UpdateTodoAsync(Todo todo)
    {
        await _repository.UpdateTodoAsync(todo);
    }
}
