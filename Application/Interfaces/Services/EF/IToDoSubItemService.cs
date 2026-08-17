using Application.DTOs.ToDoSubItem;
using Application.Models.Pagination;

namespace Application.Interfaces.Services.EF
{
    public interface IToDoSubItemService
    {
        Task<ToDoSubItemDto> AddAsync(CreateToDoSubItemDto createToDoSubItemDto, CancellationToken token = default);
        Task DeleteAsync(int id, CancellationToken token = default);
        Task<ToDoSubItemDto> UpdateAsync(UpdateToDoSubItemDto updateToDoSubItemDto, CancellationToken token = default);
        Task<ToDoSubItemDto?> GetByIdAsync(int id, CancellationToken token = default);
        Task<PagedResult<ToDoSubItemDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default);
    }
}
