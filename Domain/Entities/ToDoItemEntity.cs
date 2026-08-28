namespace Domain.Entities
{
    public class ToDoItemEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsMyDay { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public int? ToDoListId { get; set; }
        public ToDoListEntity? ToDoList { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public ICollection<ToDoSubItemEntity> SubToDoItems { get; set; } = new List<ToDoSubItemEntity>();
    }
}