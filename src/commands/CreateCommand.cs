using System.CommandLine;
using todo.src.model;
using todo.src.services;
using todo.src.utils;

namespace todo.src.commands;

public class CreateCommand : Command
{
    private readonly ITodoService _service;
    public CreateCommand(ITodoService service)
        : base("add", "Add a new item to the list")
    {
        _service = service;

        // Adiciona um novo argumento ao comando
        var titleArgument = new Argument<string[]>("title", "Task title")
        {
            // Exige pelo menos um titulo
            Arity = ArgumentArity.OneOrMore
        };

        AddArgument(titleArgument);

        // Diz o que o comando vai fazer ao ser chamado
        this.SetHandler(async (string[] title) =>
        {
            string titleFormat = string.Join(" ", title);

            string formatter = titleFormat[0].ToString().ToUpper() + titleFormat[1..].ToLower().Replace(",", "");
            var newTodo = new Todo { Title = formatter };

            await _service.AddTodoAsync(newTodo); // Adicionar uma nova tarefa
            ColorConsole.HighlightMessage("Task successfully added! ", ConsoleColor.Green);
            ViewList.ViewListDetail([ newTodo ]);

        }, titleArgument);
    }
}