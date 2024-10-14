using System.CommandLine;
using System.Text.Json;
using todo.src.model;
using todo.src.utils;

namespace todo.src.commands;

public class DeleteCommand : Command
{
    private static readonly JsonSerializerOptions s_writeOptions = new()
    {
        WriteIndented = true
    };
    public const string FilePath = "todos.json";
    public DeleteCommand()
        : base("remove", "Removes a task from the task list.")
    {
        var idArgument = new Argument<int>("id", "Provide the task id.");
        idArgument.AddValidator(result =>
        {
            try
            {
                if (result.GetValueOrDefault<int>() < 1)
                    result.ErrorMessage = "The id cannot be 0 or a negative number.";
            }
            catch (Exception)
            {
                result.ErrorMessage = "Enter a valid number.";
            }

        });
        AddArgument(idArgument);

        this.SetHandler((int id) =>
        {
            if (!File.Exists(FilePath))
            {
                ColorConsole.HighlightMessage
                (
                    "Error: The file 'todos.json' could not be found.",
                    ConsoleColor.Red
                );
                Environment.Exit(1);
                return;
            }

            // Carrega o arquivo json
            string jsonString = File.ReadAllText(FilePath);
            List<Todo> todos = JsonSerializer.Deserialize<List<Todo>>(jsonString)!;

            // Ordena a lista de todos por ID
            todos = todos.OrderBy(t => t.Id).ToList();

            // Usa a pesquisa binaria para encontrar o indice da tarefa com o ID fornecido
            int index = BinarySearch(todos, id);

            if (index >= 0)
            {
                // Remove a tarefa
                var removedTodo = todos[index];
                todos.RemoveAt(index);

                // Salva o JSON atualizado
                var updateJson = JsonSerializer.Serialize(todos, s_writeOptions);
                File.WriteAllText(FilePath, updateJson);

                ColorConsole.HighlightMessage($"Task with id {removedTodo.Id} has been removed.", ConsoleColor.Green);
            }
            else
            {
                ColorConsole.HighlightMessage($"Task with id {id} not found.", ConsoleColor.Yellow);
            }
        }, idArgument);
    }

    private static int BinarySearch(List<Todo> todos, int id)
    {
        int left = 0;
        int right = todos.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;

            if (todos[mid].Id == id) return mid; // Encontrou o item

            if (todos[mid].Id < id)
            {
                left = mid + 1; // Busca na metade da direita
            }
            else
            {
                right = mid - 1; // Busca na metade da esquerda
            }
        }

        return -1; // Não encontrou o item
    }
}