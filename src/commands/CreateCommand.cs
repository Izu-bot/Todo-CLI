using System.CommandLine;
using System.IO.Compression;
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
        : base("add", "Adiciona um novo item na lista.")
    {
        // Adiciona um novo argumento ao comando
        var titleArgument = new Argument<string[]>("title", "Titulo da tarefa")
        {
            // Exige pelo menos um titulo
            Arity = ArgumentArity.OneOrMore
        };
        AddArgument(titleArgument);

        // Diz o que o comando vai fazer ao ser chamado
        this.SetHandler((string[] titles) =>
        {
            if(titles.Length != 0) Console.WriteLine("✅ Adicionado");

            foreach (var title in titles)
            {
                string formatter = title[0].ToString().ToUpper() + title[1..].ToLower();

                var newTodo = new Todo { Title = formatter };

                // Adiciona o novo todo ao arquivo JSON de Todos
                AddTodoToFile(newTodo);
                ColorConsole.HighlightMessage($"Titulo: {formatter}, Data: {newTodo.CreatedAt.ToString("d")}", ConsoleColor.Green);
            }

        }, titleArgument);
    }

    private static void AddTodoToFile(Todo todo)
    {
        List<Todo> todos;

        // Verifica se o arquivo existe e carrega o conteúdo existente
        if (File.Exists(FilePath))
        {
            var jsonData = File.ReadAllText(FilePath);
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