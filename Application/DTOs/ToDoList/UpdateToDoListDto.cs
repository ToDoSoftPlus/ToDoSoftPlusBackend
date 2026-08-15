namespace Application.DTOs.ToDoList
{
    public class UpdateToDoListDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
