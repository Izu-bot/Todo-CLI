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
        var idArgument = new Argument<int>("id", "The ID of the task you want to change.");
        var titleArgument = new Argument<string>("title", "New title you want to assign to the task.");

        this.AddArgument(idArgument);
        this.AddArgument(titleArgument);

        this.SetHandler(( int id, string title) =>
        {
            if (!File.Exists(FilePath))
            {
                ColorConsole.HighlightMessage(
                    "Error: The file 'todos.json' could not be found.",
                    ConsoleColor.Red
                    );
                Environment.Exit(1);
                return;
            }

            var todosJson = File.ReadAllText(FilePath);
            var todos = JsonSerializer.Deserialize<List<Todo>>(todosJson);

            if (todos == null)
            {
                ColorConsole.HighlightMessage(
                    "Error: The file 'todos.json' could not be found.",
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

            // Atualizar a tarefa conforme as opções passadas
            if (title != null)
            {
                todoToUpdate.Title = title;
            }

            // Serializar para JSON
            var updateTodosJson = JsonSerializer.Serialize(todos, s_writeOptions);
            File.WriteAllText(FilePath, updateTodosJson);

            ColorConsole.HighlightMessage("Task successfully updated!", ConsoleColor.Green);
        }, idArgument, titleArgument);
    }
}


