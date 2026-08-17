using Application.DTOs.ToDoSubItem;
using Application.Exceptions;
using Application.Interfaces.Services.EF;
using Application.Interfaces.Services.Identity;
using Application.Interfaces.UnitOfWork;
using Application.Models.Pagination;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.EF
{
    public class ToDoSubItemService : IToDoSubItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly int _currentUserId;

        public ToDoSubItemService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserId = currentUserService.UserId;
        }

        public async Task<ToDoSubItemDto> AddAsync(CreateToDoSubItemDto createToDoSubItemDto, CancellationToken token = default)
        {
            if (_unitOfWork.ToDoItemRepository.GetByIdAsync(_currentUserId, createToDoSubItemDto.ToDoItemId, token) is null)
            {
                throw new NotFoundException($"To-do sub-item with ID '{createToDoSubItemDto.ToDoItemId}' not found.");
            }

            var entity = _mapper.Map<ToDoSubItemEntity>(createToDoSubItemDto);
            _unitOfWork.ToDoSubItemRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync(token);
            return _mapper.Map<ToDoSubItemDto>(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken token = default)
        {
            var entity = await _unitOfWork.ToDoSubItemRepository.GetByIdAsync(_currentUserId, id, token);

            if (entity is null) 
            {
                throw new NotFoundException($"To-do sub-item with ID '{id}' not found.");
            }

            _unitOfWork.ToDoSubItemRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task<PagedResult<ToDoSubItemDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default)
        {
            var toDoSubItems = await _unitOfWork.ToDoSubItemRepository.GetAllAsync(_currentUserId, paginationRequest.Page, paginationRequest.PageSize, token);
            return _mapper.Map<PagedResult<ToDoSubItemDto>>(toDoSubItems);
        }

        public async Task<ToDoSubItemDto?> GetByIdAsync(int id, CancellationToken token = default)
        {
            var entity = await _unitOfWork.ToDoSubItemRepository.GetByIdAsync(_currentUserId, id, token);

            if (entity is null)
            {
                throw new NotFoundException($"To-do sub-item with Id '{id}' not found.");
            }

            return entity is not null ? _mapper.Map<ToDoSubItemDto>(entity) : null;
        }

        public async Task<ToDoSubItemDto> UpdateAsync(UpdateToDoSubItemDto updateToDoSubItemDto, CancellationToken token = default)
        {
            var entity = await _unitOfWork.ToDoSubItemRepository.GetByIdAsync(_currentUserId, updateToDoSubItemDto.Id, token);

            if (entity is null)
            {
                throw new NotFoundException($"To-do sub-item with ID '{updateToDoSubItemDto.Id}' not found.");
            }

            _mapper.Map(updateToDoSubItemDto, entity);
            _unitOfWork.ToDoSubItemRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(token);
            return _mapper.Map<ToDoSubItemDto>(entity);
        }
    }
}
