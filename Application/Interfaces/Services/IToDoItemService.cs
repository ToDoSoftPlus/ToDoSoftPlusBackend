using Application.DTOs.ToDoItem;
using Application.Models.Pagination;

namespace Application.Interfaces.Services
{
    public interface IToDoItemService
    {
        Task AddAsync(CreateToDoItemDto createToDoItemDto, CancellationToken token = default);
        Task DeleteAsync(int id, CancellationToken token = default);
        Task UpdateAsync(UpdateToDoItemDto updateToDoItemDto, CancellationToken token = default);
        Task<ToDoItemDto?> GetByIdAsync(int id, CancellationToken token = default);
        Task<PagedResult<ToDoItemDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default);
    }
}
