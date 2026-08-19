using Application.DTOs.ToDoItem;
using Application.Models.Pagination;

namespace Application.Interfaces.Services.EF
{
    public interface IToDoItemService
    {
        Task<ToDoItemDto> AddAsync(CreateToDoItemDto createToDoItemDto, CancellationToken token = default);
        Task DeleteAsync(int id, CancellationToken token = default);
        Task<ToDoItemDto> UpdateAsync(UpdateToDoItemDto updateToDoItemDto, CancellationToken token = default);
        Task<ToDoItemDto> GetByIdAsync(int id, CancellationToken token = default);
        Task<PagedResult<ToDoItemDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default);
    }
}
