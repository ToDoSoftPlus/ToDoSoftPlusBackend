using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using Infrastructure.DbContext;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IToDoItemRepository ToDoItemRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IToDoItemRepository toDoItemRepository)
        {
            _context = context;
            ToDoItemRepository = toDoItemRepository;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
