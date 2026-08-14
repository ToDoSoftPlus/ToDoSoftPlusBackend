namespace Domain.Entities
{
    public class MyDayListEntity
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public int ToDoItemId { get; set; }
        public ToDoItemEntity ToDoItem { get; set; } = null!;

        public DateOnly Date { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
