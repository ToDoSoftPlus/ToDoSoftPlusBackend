using Application.Models.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IToDoListRepository
    {
        Task<ToDoListEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<ToDoListEntity>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        void Add(ToDoListEntity item);
        void Update(ToDoListEntity item);
        void Delete(ToDoListEntity item);
    }
}
