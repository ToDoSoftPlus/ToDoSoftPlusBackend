using Application.Models.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IToDoItemRepository
    {
        Task<ToDoItemEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<ToDoItemEntity>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        void Add(ToDoItemEntity item);
        void Update(ToDoItemEntity item);
        void Delete(ToDoItemEntity item);
    }
}
