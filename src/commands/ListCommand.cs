using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.Json;
using todo.src.model;
using todo.src.utils;

namespace todo.src.commands;

public class ListCommand : Command
{
    public const string FilePath = "todos.json";

    public ListCommand()
        : base("list", "List all taks")
    {
        var idOption = new Option<int>(
            name: "--id",
            description: "An option to search by Ids."
        );
        idOption.AddAlias("-i");

        AddOption(idOption);

        this.SetHandler((id) =>
        {
            if (!File.Exists("todos.json"))
            {
                ColorConsole.HighlightMessage
                (
                    "Error: The file 'todos.json' could not be found.",
                    ConsoleColor.Red
                );
                Environment.Exit(1);
                return;
            }

            string jsonString = File.ReadAllText(FilePath);
            List<Todo> todos = JsonSerializer.Deserialize<List<Todo>>(jsonString)!;

            if (todos.Count == 0)
            {
                ColorConsole.HighlightMessage("No tasks found", ConsoleColor.Yellow);
            }
            else
            {
                foreach (var todo in todos)
                {
                    string status = todo.IsDone ? "Completed" : "Not completed";

                    if (todo.Id == id || id == 0)
                    {
                        ColorConsole.HighlightMessage(
                        $"ID: {todo.Id}, Title: {todo.Title}, Status: {status}, created at: {todo.CreatedAt:d}",
                        ConsoleColor.Blue
                        );
                    }
                }
            }
        }, idOption);

    }
}