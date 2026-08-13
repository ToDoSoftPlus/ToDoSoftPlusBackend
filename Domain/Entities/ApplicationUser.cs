using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ICollection<ToDoCategoryEntity> Categories { get; set; } = new List<ToDoCategoryEntity>();
        public ICollection<ToDoItemEntity> Items { get; set; } = new List<ToDoItemEntity>();
    }
}
