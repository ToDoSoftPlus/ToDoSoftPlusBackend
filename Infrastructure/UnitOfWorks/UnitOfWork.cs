using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using Infrastructure.DbContext;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IToDoItemRepository ToDoItemRepository { get; }

        public IToDoListRepository ToDoListRepository { get; }

        public IToDoSubItemRepository ToDoSubItemRepository { get; }

        public UnitOfWork(
            ApplicationDbContext context, 
            IToDoItemRepository toDoItemRepository, 
            IToDoListRepository toDoListRepository, 
            IToDoSubItemRepository toDoSubItemRepository)
        {
            _context = context;
            ToDoItemRepository = toDoItemRepository;
            ToDoListRepository = toDoListRepository;
            ToDoSubItemRepository = toDoSubItemRepository;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
