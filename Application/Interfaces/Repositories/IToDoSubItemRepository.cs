using Application.Models.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IToDoSubItemRepository
    {
        Task<ToDoSubItemEntity?> GetByIdAsync(int userId, int id, CancellationToken cancellationToken = default);
        Task<PagedResult<ToDoSubItemEntity>> GetAllAsync(int userId, int page, int pageSize, CancellationToken cancellationToken = default);
        void Add(ToDoSubItemEntity item);
        void Update(ToDoSubItemEntity item);
        void Delete(ToDoSubItemEntity item);
    }
}
