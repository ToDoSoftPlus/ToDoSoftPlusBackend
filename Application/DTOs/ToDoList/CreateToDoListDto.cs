namespace Application.DTOs.ToDoList
{
    public class CreateToDoListDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int UserId { get; set; }
    }
}
