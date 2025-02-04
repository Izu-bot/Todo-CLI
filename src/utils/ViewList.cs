using System.Reflection;
using Spectre.Console;
using todo.src.model;

namespace todo.src.utils;

public class ViewList
{
    public static void ViewListDetail(List<Todo> todos)
    {
        // Determinar o status como texto
        var table = new Table();
        var columnNames = new Dictionary<string, string>
        {
            {"IsDone", "Done"},
            {"CreatedAt", "Created At"}
        };

        PropertyInfo[] propertyInfos = typeof(Todo).GetProperties();

        foreach (var propertys in propertyInfos)
        {
            string columnName = columnNames.TryGetValue(propertys.Name, out string? value) ? value : propertys.Name;
            table.AddColumn(columnName);
        }

        foreach (var todo in todos)
        {
            table
            .AddRow(propertyInfos.Select(p =>
            {
                var value = p.GetValue(todo);

                if (value is bool boolValue) return boolValue ? "Completed" : "Pending";

                if (value is DateTime dateTime) return dateTime.ToString("d");

                return value?.ToString() ?? "";
            })
            .ToArray());
        }

        AnsiConsole.Write(table);
    }
}

