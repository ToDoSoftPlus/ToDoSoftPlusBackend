using Application.Interfaces.Repositories;
using Application.Models.Pagination;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ToDoSubItemRepository : IToDoSubItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ToDoSubItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ToDoSubItemEntity item)
        {
            _context.Add(item);
        }

        public void Delete(ToDoSubItemEntity item)
        {
            _context.Remove(item);
        }

        public async Task<PagedResult<ToDoSubItemEntity>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.ToDoSubItems.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<ToDoSubItemEntity>
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

        public async Task<ToDoSubItemEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ToDoSubItems
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public void Update(ToDoSubItemEntity item)
        {
            _context.Update(item);
        }
    }
}
