using Application.DTOs.ToDoList;
using Application.Models.Pagination;
using Application.Models.Filters;

namespace Application.Interfaces.Services.EF
{
    public interface IToDoListService
    {
        Task<ToDoListDto> AddAsync(CreateToDoListDto createToDoListDto, CancellationToken token = default);
        Task DeleteAsync(int id, CancellationToken token = default);
        Task<ToDoListDto> UpdateAsync(UpdateToDoListDto updateToDoListDto, CancellationToken token = default);
        Task<ToDoListDto> GetByIdAsync(int id, CancellationToken token = default);
        Task<PagedResult<ToDoListDto>> GetAllAsync(
            PaginationRequest paginationRequest, FiltetingListsRequest filterListsRequest, CancellationToken token = default);
        Task<PagedResult<ToDoSidebarListDto>> GetSidebarListsAsync(
            PaginationRequest paginationRequest, FiltetingListsRequest filterListsRequest, CancellationToken token = default);
        Task<PagedResult<ToDoSidebarListDto>> SearchSidebarListsAsync(
            string title, PaginationRequest paginationRequest, FiltetingListsRequest filterListsRequest, CancellationToken token = default);
    }
}
