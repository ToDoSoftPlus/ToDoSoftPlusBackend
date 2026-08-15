using Application.DTOs.ToDoList;
using Application.Models.Pagination;

namespace Application.Interfaces.Services
{
    public interface IToDoListService
    {
        Task<ToDoListDto> AddAsync(CreateToDoListDto createToDoListDto, CancellationToken token = default);
        Task DeleteAsync(int id, CancellationToken token = default);
        Task<ToDoListDto> UpdateAsync(UpdateToDoListDto updateToDoListDto, CancellationToken token = default);
        Task<ToDoListDto?> GetByIdAsync(int id, CancellationToken token = default);
        Task<PagedResult<ToDoListDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default);
    }
}
