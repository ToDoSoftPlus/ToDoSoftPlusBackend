namespace Application.DTOs.ToDoSubItem
{
    public class CreateToDoSubItemDto
    {
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int ToDoItemId { get; set; }
    }
}
