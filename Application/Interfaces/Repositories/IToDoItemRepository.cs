using Application.Models.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IToDoItemRepository
    {
        Task<ToDoItemEntity?> GetByIdAsync(int userId, int id, CancellationToken cancellationToken = default);
        Task<PagedResult<ToDoItemEntity>> GetAllAsync(int userId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<PagedResult<ToDoItemEntity>> GetItemsInListAsync(int userId, int listId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<int> GetMyDayCountItemsAsync(int userId, CancellationToken cancellationToken = default);
        Task<int> GetImportantCountItemsAsync(int userId, CancellationToken cancellationToken = default);
        Task<int> GetTaskCountItemsAsync(int userId, CancellationToken cancellationToken = default);
        void Add(ToDoItemEntity item);
        void Update(ToDoItemEntity item);
        void Delete(ToDoItemEntity item);
    }
}
