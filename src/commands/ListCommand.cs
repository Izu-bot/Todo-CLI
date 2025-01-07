using System.CommandLine;
using todo.src.model;
using todo.src.services;
using todo.src.utils;

namespace todo.src.commands;

public class ListCommand : Command
{
    private readonly ITodoService _service;
    public ListCommand(ITodoService service)
        : base("list", "List all taks")
    {
        _service = service;

        var titleOptions = new Option<string>(
            name: "--name",
            description: "An option to search by names"
        );
        titleOptions.AddAlias("-n");

        AddOption(titleOptions);

        this.SetHandler((name) =>
        {

            Console.WriteLine("{0, -5} {1, -25} {2, -10} {3, -15}", "Id", "Title", "Done", "Created At");
            Console.WriteLine(new string('-', 55));
            
            if (String.IsNullOrWhiteSpace(name))
            {
                var todo = _service.GetAll(); 
                
                foreach (Todo item in todo)
                {
                    // Muda a logica como aparece o valor bool
                    var teste = item.IsDone ? "Concluido" : "Pendente";
                    
                    // Formata a saida no console
                    ColorConsole.HighlightMessage(
                        $"{item.Id, -5} {item.Title, -25} {teste, -10} {item.CreatedAt.ToString("d"), -15}", ConsoleColor.Blue);
                }
            }
            else
            {
                var todo = _service.GetTitle(name);
                
                foreach (Todo item in todo!)
                {
                    // Muda a logica como aparece o valor bool
                    var teste = item.IsDone ? "Concluido" : "Pendente";
                    
                    // Formata a saida no console
                    ColorConsole.HighlightMessage(
                        $"{item.Id, -5} {item.Title, -25} {teste, -10} {item.CreatedAt.ToString("d"), -15}", ConsoleColor.Blue);
                }
            }

        }, titleOptions);
    }
}