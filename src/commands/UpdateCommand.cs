using System.CommandLine;
using System.Text.Json;
using System.Text.Json.Serialization;
using todo.src.model;
using todo.src.utils;

namespace todo.src.commands;

public class UpdateCommand : Command
{
    private static readonly JsonSerializerOptions s_writeOptions = new()
    {
        WriteIndented = true
    };
    public const string FilePath = "todos.json";
    public UpdateCommand()
        : base("update", "Update a task")
    {
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
        )
        {
            IsRequired = true
        };
        doneOption.AddAlias("-d");
        this.AddOption(doneOption);

        this.AddArgument(idArgument);

        this.SetHandler((int id, string title, string done) =>
        {
            // Verifica se não existe o arquivo 'todos.json'
            if (!File.Exists(FilePath))
            {
                ColorConsole.HighlightMessage(
                    "Error: The file 'todos.json' could not be found",
                    ConsoleColor.Red
                    );
                Environment.Exit(1);
                return;
            }

            // Deserializa o json para C#
            var todosJson = File.ReadAllText(FilePath);
            var todos = JsonSerializer.Deserialize<List<Todo>>(todosJson);

            // Verifica se contém algo na lista de todos
            if (todos == null)
            {
                ColorConsole.HighlightMessage(
                    "Error: No task to update",
                    ConsoleColor.Red
                );
                return;
            }

            // Procura a tarefa pelo ID
            var todoToUpdate = todos.FirstOrDefault(t => t.Id == id);
            if (todoToUpdate == null)
            {
                ColorConsole.HighlightMessage(
                    $"Error: The task with id:{id} was not found",
                    ConsoleColor.Red
                );
                return;
            }

            // Atualiza a propriedade done e converte done para um boolean no json
            if (!string.IsNullOrEmpty(done))
            {
                switch (done.ToLower())
                {
                    case "yes":
                    case "y":
                        todoToUpdate.IsDone = true;
                        break;

                    case "no":
                    case "n":
                        todoToUpdate.IsDone = false;
                        break;

                    default:
                        ColorConsole.HighlightMessage(
                            "Error: The value of the --done flag must be 'yes', 'no', 'y', or 'n'",
                            ConsoleColor.Red
                        );
                        return;
                }
            }

            // Verifica se o title está vazio, caso não, atualiza no json
            if (!string.IsNullOrEmpty(title))
            {
                todoToUpdate.Title = title;
            }

            // Serializar para JSON
            var updateTodosJson = JsonSerializer.Serialize(todos, s_writeOptions);
            File.WriteAllText(FilePath, updateTodosJson);

            ColorConsole.HighlightMessage("Task successfully updated!", ConsoleColor.Green);
        }, idArgument, titleOption, doneOption);
    }
}


