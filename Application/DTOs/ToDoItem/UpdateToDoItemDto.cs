using Domain.Entities;

namespace Application.DTOs.ToDoItem
{
    public class UpdateToDoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCompleted { get; set; }
        public int ToDoListId { get; set; }
    }
}
