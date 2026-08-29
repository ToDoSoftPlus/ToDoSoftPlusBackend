using Application.DTOs.ToDoItem;

namespace Application.DTOs.ToDoList
{
    public class ToDoListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }
        //public ApplicationUser User { get; set; } = null!;
        public ICollection<ToDoItemDto> ToDoItemsList { get; set; } = new List<ToDoItemDto>();
    }
}
