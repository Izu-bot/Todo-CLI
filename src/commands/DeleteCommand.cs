using System.CommandLine;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        : base("remove", "Remove uma tarefa da lista de tarefas.")
    {
        var idArgument = new Argument<int>("id", "Fornecer o id da tarefa\nDica: Você pode obter o Id através do comando 'list'");
        idArgument.AddValidator(result =>
        {
            if (result.GetValueOrDefault<int>() < 1)
                result.ErrorMessage = "O id não pode ser 0 e nem um número negativo.";
        });
        AddArgument(idArgument);

        this.SetHandler((int id) => 
        {
            if (!File.Exists(FilePath))
            {
                ColorConsole.HighlightMessage
                (
                    "Erro: O arquivo 'todos.json' não foi encontrado.",
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

            if(index >= 0)
            {
                // Remove a tarefa
                var removedTodo = todos[index];
                todos.RemoveAt(index);

                // Salva o JSON atualizado
                var updateJson = JsonSerializer.Serialize(todos, s_writeOptions);
                File.WriteAllText(FilePath, updateJson);

                ColorConsole.HighlightMessage($"Tarefa com Id {removedTodo.Id} foi removida.", ConsoleColor.Green);
            }
            else
            {
                ColorConsole.HighlightMessage($"Tarefa com id {id} não foi encontrada.", ConsoleColor.Yellow);
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

            if(todos[mid].Id == id) return mid; // Encontrou o item

            if(todos[mid].Id < id)
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