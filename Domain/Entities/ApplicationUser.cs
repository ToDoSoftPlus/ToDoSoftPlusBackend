using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ToDoListEntity> ToDoLists { get; set; } = new List<ToDoListEntity>();
        public ICollection<MyDayListEntity> MyDayList { get; set; } = new List<MyDayListEntity>();  
    }
}
