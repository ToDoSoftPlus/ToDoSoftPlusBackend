using Domain.Entities;

namespace Application.DTOs.ToDoSubItem
{
    public class ToDoSubItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int ToDoItemId { get; set; }
        //public ToDoItemEntity ToDoItem { get; set; } = null!;
    }
}
