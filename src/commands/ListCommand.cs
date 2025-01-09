using System.CommandLine;
using System.Drawing;
using todo.ErrorManagement;
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

        var idOptions = new Option<int>(
            name: "--id",
            description: "An option to seach by id's"
        );
        idOptions.AddAlias("-i");

        AddOption(titleOptions);
        AddOption(idOptions);

        this.SetHandler((name, id) =>
        {

            Console.WriteLine("{0, -5} {1, -25} {2, -10} {3, -15}", "Id", "Title", "Done", "Created At");
            Console.WriteLine(new string('-', 55));
            
            if(String.IsNullOrWhiteSpace(name))
            {
                if(id != 0)
                {
                    var (status, todo) = _service.GetId(id);
                    if (status.IsSuccess() && todo != null) ViewListDetail(todo);
                    else ColorConsole.HighlightMessage($"Error code: {(int) status}\nIndicates that the resource was not found in your database. Check the Id number or Title passed in the search.", ConsoleColor.Red);  
                }
                else
                {
                    var todos = _service.GetAll();

                    foreach(Todo item in todos)
                    {
                        ViewListDetail(item);
                    }
                }
            }
            else
            {
                var (status, todos) = _service.GetTitle(name);
                
                if (status != OperationsStatus.Success) Console.WriteLine("Erro");

                foreach (Todo item in todos!)
                {
                    ViewListDetail(item);
                }
            }

        }, titleOptions, idOptions);
    }

    // Método para exibir a lista de todos
    static void ViewListDetail(Todo item)
    {
        // Determinar o status como texto
        var status = item.IsDone ? "Completed" : "Pending";

        // Formata e exibe a mensagem no console
        ColorConsole.HighlightMessage(
            $"{item.Id,-5} {item.Title,-25} {status,-10} {item.CreatedAt.ToString("dd/MM/yyyy"), -15} ", ConsoleColor.Blue);
    }
}