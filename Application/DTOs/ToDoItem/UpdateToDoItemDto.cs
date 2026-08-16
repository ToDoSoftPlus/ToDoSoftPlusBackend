using Domain.Entities;

namespace Application.DTOs.ToDoItem
{
    public class UpdateToDoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCompleted { get; set; }

        public int ToDoListId { get; set; }
        public ToDoListEntity ToDoList { get; set; } = null!;

        public int? ParentToDoItemId { get; set; }
        public ToDoItemDto? ParentToDoItem { get; set; }
        public ICollection<ToDoItemDto> SubTasks { get; set; } = new List<ToDoItemDto>();
    }
}
