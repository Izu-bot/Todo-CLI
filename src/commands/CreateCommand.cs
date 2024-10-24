using System.CommandLine;
using System.Text.Json;
using todo.src.model;
using todo.src.utils;

namespace todo.src.commands;

public class CreateCommand : Command
{
    private static readonly JsonSerializerOptions s_writeOptions = new()
    {
        WriteIndented = true
    };
    public const string FilePath = "todos.json";
    public CreateCommand()
        : base("add", "Adds a new item to the list")
    {
        // Adiciona um novo argumento ao comando
        var titleArgument = new Argument<string[]>("title", "Task title")
        {
            // Exige pelo menos um titulo
            Arity = ArgumentArity.OneOrMore
        };

        titleArgument.AddValidator(result =>
        {
            var titles = result.GetValueForArgument(titleArgument); // Captura a coleção de argumentos
            foreach (var title in titles) // Itera sobre cada titulo
            {
                if (string.IsNullOrWhiteSpace(title)) // Verifica se está vazio ou com espaços em branco
                {
                    result.ErrorMessage = "Titles must not be empty";
                    return;
                }
            }
        });
        AddArgument(titleArgument);

        // Diz o que o comando vai fazer ao ser chamado
        this.SetHandler((string[] titles) =>
        {
            foreach (var title in titles)
            {
                // Deixa a primeira letra maiuscula e as demais minuscula
                string formatter = title[0].ToString().ToUpper() + title[1..].ToLower().Replace(",", "");
                var newTodo = new Todo { Title = formatter };

                // Adiciona o novo todo ao arquivo JSON de Todos
                AddTodoToFile(newTodo);
                ColorConsole.HighlightMessage($"Title: {formatter}, Date: {newTodo.CreatedAt:d}", ConsoleColor.Green);
            }
            // Console.WriteLine("✅ Adicionado");
        }, titleArgument);
    }

    private static void AddTodoToFile(Todo todo)
    {
        List<Todo> todos;

        // Verifica se o arquivo existe e carrega o conteúdo existente
        if (File.Exists(FilePath))
        {
            var jsonData = File.ReadAllText(FilePath);
            // deserializa a string jsonData em uma lista de objetos Todo se retornar null garante uma lista vazia
            todos = JsonSerializer.Deserialize<List<Todo>>(jsonData) ?? [];
        }
        else
        {
            todos = [];
        }

        // Define o próximo ID único (pega o maior ID existente e incrementa)
        var nextId = todos.Count != 0 ? todos.Max(t => t.Id) + 1 : 1;
        todo.Id = nextId;

        // Adiciona o novo todo à lista
        todos.Add(todo);

        // Salva de volta no arquivo JSON
        var updateJson = JsonSerializer.Serialize(todos, s_writeOptions);
        File.WriteAllText(FilePath, updateJson);
    }
}