using Application.DTOs.ToDoItem;
using Application.Exceptions;
using Application.Interfaces.Services.EF;
using Application.Interfaces.Services.Identity;
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
        private readonly int _currentUserId;

        public ToDoItemService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserId = currentUserService.UserId;
        }

        public async Task<ToDoItemDto> AddAsync(CreateToDoItemDto createToDoItemDto, CancellationToken token = default)
        {
            var list = await _unitOfWork.ToDoListRepository.GetByIdAsync(_currentUserId, createToDoItemDto.ToDoListId, token);

            if (list is null)
            {
                throw new NotFoundException($"ToDoList with Id '{createToDoItemDto.ToDoListId}' not found.");
            }

            var entity = _mapper.Map<ToDoItemEntity>(createToDoItemDto);
            _unitOfWork.ToDoItemRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync(token);
            return _mapper.Map<ToDoItemDto>(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken token = default)
        {
            var entity = await _unitOfWork.ToDoItemRepository.GetByIdAsync(_currentUserId, id, token);

            if (entity is null)
            {
                throw new NotFoundException($"ToDoItem with Id '{id}' not found.");
            }

            _unitOfWork.ToDoItemRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task<PagedResult<ToDoItemDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default)
        {
            var toDoItems = await _unitOfWork.ToDoItemRepository.GetAllAsync(_currentUserId, paginationRequest.Page, paginationRequest.PageSize, token);
            return _mapper.Map<PagedResult<ToDoItemDto>>(toDoItems);
        }

        public async Task<ToDoItemDto> GetByIdAsync(int id, CancellationToken token = default)
        {
            var entity = await _unitOfWork.ToDoItemRepository.GetByIdAsync(_currentUserId, id, token);

            if (entity is null)
            {
                throw new NotFoundException($"ToDoItem with Id '{id}' not found.");
            }

            return _mapper.Map<ToDoItemDto>(entity);
        }

        public async Task<ToDoItemDto> UpdateAsync(UpdateToDoItemDto updateToDoItemDto, CancellationToken token = default)
        {
            var entity = await _unitOfWork.ToDoItemRepository.GetByIdAsync(_currentUserId, updateToDoItemDto.Id, token);

            if (entity is null)
            {
                throw new NotFoundException($"ToDoItem with Id '{updateToDoItemDto.Id}' not found.");
            }

            _mapper.Map(updateToDoItemDto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.ToDoItemRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(token);
            return _mapper.Map<ToDoItemDto>(entity);
        }
    }
}
