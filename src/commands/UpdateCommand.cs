using System.CommandLine;
using todo.ErrorManagement;
using todo.src.services;
using todo.src.utils;

namespace todo.src.commands;

public class UpdateCommand : Command
{
    private readonly ITodoService _service;
    public UpdateCommand(ITodoService service)
        : base("update", "Update a task")
    {
        _service = service;

        var idArgument = new Argument<int>("id", "The ID of the task you want to change");

        var titleOption = new Option<string>(
            name: "--title",
            description: "Can be used to change the task title"
        );

        titleOption.AddAlias("-t");
        this.AddOption(titleOption);

        var doneOption = new Option<string>(
            name: "--done",
            description: "Can be used to mark a task as completed"
        );

        doneOption.AddAlias("-d");
        this.AddOption(doneOption);

        this.AddArgument(idArgument);

        this.SetHandler((int id, string title, string done) =>
        {
            // Procura a tarefa pelo ID
            // var searchTodo = _service.GetId(id) ?? throw new InvalidOperationException($"Task with ID {id} not found.");
            var (_, searchTodo) = _service.GetId(id);

            if (searchTodo == null)
            {
                ColorConsole.HighlightMessage("Indicates that the resource was not found in your database. Check the Id number or Title passed in the search.", ConsoleColor.Red);
                return;
            }

            // Atualiza title se não for vazio
            if (!string.IsNullOrWhiteSpace(title)) searchTodo.Title = title.Trim();

            if (!string.IsNullOrWhiteSpace(done))
            {
                // Normaliza a entrada tirando os espaços
                string normalizedInput = done.Trim();

                // Switch case para verificar possiveis entradas permitidas
                switch (normalizedInput.ToLower())
                {
                    case "y":
                        searchTodo.IsDone = true;
                        break;
                    case "yes":
                        searchTodo.IsDone = true;
                        break;
                    case "n":
                        searchTodo.IsDone = false;
                        break;
                    case "not":
                        searchTodo.IsDone = false;
                        break;
                    default:
                        ColorConsole.HighlightMessage("Enter the possible entries “y” or “n”.", ConsoleColor.Red);
                        return;
                }
            }
            string isCompleted = searchTodo.IsDone ? "Completed" : "Pending";

            // Chama o serviço para persistir no banco
            var status = _service.UpdateTodo(searchTodo);

            if (status.IsSuccess())
            {
                ColorConsole.HighlightMessage(
                    $"Task successfully updated!\nUpdate Task: ID: {searchTodo.Id} Title: {searchTodo.Title}, Done: {isCompleted}, Created At: {searchTodo.CreatedAt:d}, Last Update: {DateTime.Now:d}"
                    , ConsoleColor.Green
                );
            }

        }, idArgument, titleOption, doneOption);
    }
}


