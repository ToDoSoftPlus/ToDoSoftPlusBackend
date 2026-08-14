using Application.DTOs.ToDoItem;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Application.Models.Pagination;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.EF
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ToDoItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task AddAsync(CreateToDoItemDto createToDoItemDto, CancellationToken token = default)
        {
            var entity = _mapper.Map<ToDoItemEntity>(createToDoItemDto);

            try
            {
                _unitOfWork.ToDoItemRepository.Add(entity);
                return _unitOfWork.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task DeleteAsync(int id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ToDoItemDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<ToDoItemDto?> GetByIdAsync(int id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateToDoItemDto updateToDoItemDto, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
