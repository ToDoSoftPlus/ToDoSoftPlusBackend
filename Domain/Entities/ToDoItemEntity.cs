namespace Domain.Entities
{
    public class ToDoItemEntity
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsImportant { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeterminateDate { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}