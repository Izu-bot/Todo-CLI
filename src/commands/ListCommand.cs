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
        : base("list", "Lista todas as tarefas")
    {
        this.SetHandler(() =>
        {
            if (!File.Exists("todos.json"))
            {
                ColorConsole.HighlightMessage
                (
                    "Erro: O arquivo 'todos.json' não foi encontrado.",
                    ConsoleColor.Red
                );
                Environment.Exit(1);
                return;
            }

            string jsonString = File.ReadAllText(FilePath);
            List<Todo> todos = JsonSerializer.Deserialize<List<Todo>>(jsonString)!;

            if (todos.Count == 0)
            {
                ColorConsole.HighlightMessage("Nenhuma tarefa encontrada", ConsoleColor.Yellow);
            }
            else
            {
                foreach (var todo in todos)
                {
                    string status = todo.IsDone ? "Concluido" : "Não Concluido";

                    ColorConsole.HighlightMessage(
                        $"ID: {todo.Id}, Titulo: {todo.Title}, Status: {status}, Criado: {todo.CreatedAt.ToString("d")}",
                        ConsoleColor.Blue
                        );
                }
            }
        });

    }
}