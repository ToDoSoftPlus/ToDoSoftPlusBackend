namespace Domain.Entities
{
    public class ToDoCategoryEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public ICollection<ToDoItemEntity> ToDoItemsList { get; set; } = new List<ToDoItemEntity>();
    }
}
