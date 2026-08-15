namespace Application.DTOs.ToDoSubItem
{
    public class UpdateToDoSubItemDto
    {
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
