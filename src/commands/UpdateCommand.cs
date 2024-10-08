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
        : base("update", "Atualiza uma tarefa")
    {
        var idArgument = new Argument<int>("id", "O ID da tarefa que deseja alterar.");
        var titleArgument = new Argument<string>("title", "Novo titulo que deseja atribui a tarefa");

        this.AddArgument(idArgument);
        this.AddArgument(titleArgument);

        this.SetHandler(( int id, string title) =>
        {
            if (!File.Exists(FilePath))
            {
                ColorConsole.HighlightMessage(
                    "Erro: O arquivo 'todos.json' não foi encontrado.",
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
                    "Erro: Não há tarefas para atualizar",
                    ConsoleColor.Red
                );
                return;
            }

            // Procura a tarefa pelo ID
            var todoToUpdate = todos.FirstOrDefault(t => t.Id == id);
            if (todoToUpdate == null)
            {
                ColorConsole.HighlightMessage(
                    $"Erro: A tarefa com o id:{id} não foi encontrado",
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

            ColorConsole.HighlightMessage("Tarefa atualizada com sucesso!", ConsoleColor.Green);
        }, idArgument, titleArgument);
    }
}


