using System.CommandLine;
using todo.src.services;

namespace todo.src.commands;

public class DeleteCommand : Command
{
    private readonly ITodoService _service;
    public DeleteCommand(ITodoService service)
        : base("remove", "Removes a task from the task list")
    {
        _service = service;

        var idArgument = new Argument<int>("id", "Provide the task id"); // Cria um novo argumento
        idArgument.AddValidator(result =>
        {
            try
            {
                // Verifica se o numero pesquisado é menor que 1
                if (result.GetValueOrDefault<int>() < 1)
                    result.ErrorMessage = "The id cannot be 0 or a negative number";
            }
            catch (Exception)
            {
                result.ErrorMessage = "Enter a valid number";
            }

        });
        AddArgument(idArgument);

        this.SetHandler((int id) =>
        {
            _service.DeleteTodo(id);

        }, idArgument);
    }
}