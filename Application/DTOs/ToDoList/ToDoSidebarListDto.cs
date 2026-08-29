namespace Application.DTOs.ToDoList
{
    public class ToDoSidebarListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CountItems { get; set; }
    }
}
