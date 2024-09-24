namespace todo.src.model;
public class Todo
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsDone { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
