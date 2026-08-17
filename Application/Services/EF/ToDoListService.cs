using Application.DTOs.ToDoList;
using Application.Exceptions;
using Application.Interfaces.Services.EF;
using Application.Interfaces.Services.Identity;
using Application.Interfaces.UnitOfWork;
using Application.Models.Pagination;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.EF
{
    public class ToDoListService : IToDoListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly int _currentUserId;

        public ToDoListService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserId = currentUserService.UserId;
        }

        public async Task<ToDoListDto> AddAsync(CreateToDoListDto createToDoListDto, CancellationToken token = default)
        {
            if (await _unitOfWork.ToDoListRepository.IsExistsByTitleAndUserIdAsync(createToDoListDto.Title, _currentUserId, token))
            {
                throw new AlreadyExistsException($"A to-do list with title '{createToDoListDto.Title}' already exists.");
            }
            
            var entity = _mapper.Map<ToDoListEntity>(createToDoListDto);

            entity.UserId = _currentUserId;
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.ToDoListRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync(token);

            return _mapper.Map<ToDoListDto>(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken token = default)
        {
            if (await _unitOfWork.ToDoListRepository.GetByIdAsync(_currentUserId, id, token) is not ToDoListEntity entity)
            {
                throw new NotFoundException($"To-do list with ID '{id}' not found.");
            }

            _unitOfWork.ToDoListRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task<PagedResult<ToDoListDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default)
        {
            var toDoLists = await _unitOfWork.ToDoListRepository.GetAllAsync(_currentUserId, paginationRequest.Page, paginationRequest.PageSize, token);
            return _mapper.Map<PagedResult<ToDoListDto>>(toDoLists);
        }

        public async Task<ToDoListDto?> GetByIdAsync(int id, CancellationToken token = default)
        {
            var entity = await _unitOfWork.ToDoListRepository.GetByIdAsync(_currentUserId, id, token);
            return entity is not null ? _mapper.Map<ToDoListDto>(entity) : null;
        }

        public async Task<ToDoListDto> UpdateAsync(UpdateToDoListDto updateToDoListDto, CancellationToken token = default)
        {
            if (await _unitOfWork.ToDoListRepository.GetByIdAsync(_currentUserId, updateToDoListDto.Id, token) is not ToDoListEntity entity)
            {
                throw new NotFoundException($"To-do list with ID '{updateToDoListDto.Id}' not found.");
            }

            if (await _unitOfWork.ToDoListRepository.IsExistsByTitleAndUserIdAsync(updateToDoListDto.Title, _currentUserId, token))
            {
                throw new AlreadyExistsException($"A to-do list with title '{updateToDoListDto.Title}' already exists.");
            }

            _mapper.Map(updateToDoListDto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.ToDoListRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(token);

            return _mapper.Map<ToDoListDto>(entity);
        }
    }
}
