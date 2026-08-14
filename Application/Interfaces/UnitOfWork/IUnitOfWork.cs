using Application.Interfaces.Repositories;

namespace Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IToDoItemRepository ToDoItemRepository { get; }
        IToDoListRepository ToDoListRepository { get; }
        IToDoSubItemRepository ToDoSubItemRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
