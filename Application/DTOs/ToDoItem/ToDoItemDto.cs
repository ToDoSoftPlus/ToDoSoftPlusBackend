namespace Application.DTOs.ToDoItem
{
    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int ToDoListId { get; set; }
        //public ToDoListEntity ToDoList { get; set; } = null!;


        //public ICollection<MyDayListEntity> MyDayLists { get; set; } = new List<MyDayListEntity>();
    }
}
