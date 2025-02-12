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

       if (usuario != null)
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
            throw new NullReferenceException("Todo not found");
        }
    }

    public async Task<List<Todo>> GetTitleAsync(string title)
    {
        return await _repository.GetTitle(title).ToListAsync();
    }

    public Task UpdateTodoAsync(Todo todo)
    {
        return _repository.UpdateTodoAsync(todo);
    }
}
