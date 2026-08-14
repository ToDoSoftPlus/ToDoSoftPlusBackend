using Application.Interfaces.Repositories;
using Application.Models.Pagination;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ApplicationDbContext _context;

        public ToDoListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ToDoListEntity item)
        {
            _context.Add(item);
        }

        public void Delete(ToDoListEntity item)
        {
            _context.Remove(item);
        }

        public async Task<PagedResult<ToDoListEntity>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.ToDoLists.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<ToDoListEntity>
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

        public async Task<ToDoListEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ToDoLists
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
        }

        public void Update(ToDoListEntity item)
        {
            _context.Update(item);
        }
    }
}
