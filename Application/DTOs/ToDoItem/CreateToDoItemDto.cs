namespace Application.DTOs.ToDoItem
{
    public class CreateToDoItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int ToDoListId { get; set; }

    }
}
