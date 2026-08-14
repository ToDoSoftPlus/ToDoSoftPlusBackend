using Application.Interfaces.Repositories;

namespace Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IToDoItemRepository ToDoItemRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
