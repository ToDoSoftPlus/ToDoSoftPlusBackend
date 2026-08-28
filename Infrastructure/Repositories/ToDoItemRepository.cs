using Application.Interfaces.Repositories;
using Application.Models.Pagination;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ToDoItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ToDoItemEntity item)
        {
            _context.Add(item);
        }

        public void Delete(ToDoItemEntity item)
        {
            _context.Remove(item);
        }

        public void Update(ToDoItemEntity item)
        {
            _context.Update(item);
        }

        public async Task<PagedResult<ToDoItemEntity>> GetAllAsync(int userId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.ToDoItems
                .AsNoTracking()
                .Where(x => x.UserId == userId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<ToDoItemEntity>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }

        public async Task<ToDoItemEntity?> GetByIdAsync(int userId, int id, CancellationToken cancellationToken = default)
        {
            return await _context.ToDoItems
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedResult<ToDoItemEntity>> GetItemsInListAsync(int userId, int listId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.ToDoItems
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.ToDoListId == listId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<ToDoItemEntity>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }

        public async Task<int> GetMyDayCountItemsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.ToDoItems
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.IsMayDay == true)
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetTaskCountItemsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.ToDoItems
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.ToDoListId == null)
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetImportantCountItemsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.ToDoItems
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.IsImportant == true)
                .CountAsync(cancellationToken);
        }
    }
}
