using Application.DTOs.ToDoList;
using Application.Exceptions;
using Application.Interfaces.Services;
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

        public ToDoListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ToDoListDto> AddAsync(CreateToDoListDto createToDoListDto, CancellationToken token = default)
        {
            if (await _unitOfWork.ToDoListRepository.IsExistsByTitleAndUserIdAsync(createToDoListDto.Title, createToDoListDto.UserId, token))
            {
                throw new AlreadyExistsException($"A to-do list with title '{createToDoListDto.Title}' and user ID '{createToDoListDto.UserId}' already exists.");
            }
            
            var entity = _mapper.Map<ToDoListEntity>(createToDoListDto);
            _unitOfWork.ToDoListRepository.Add(entity);
            await _unitOfWork.SaveChangesAsync(token);
            return _mapper.Map<ToDoListDto>(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken token = default)
        {
            if (await _unitOfWork.ToDoListRepository.GetByIdAsync(id, token) is not ToDoListEntity entity)
            {
                throw new NotFoundException($"To-do list with ID '{id}' not found.");
            }

            _unitOfWork.ToDoListRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task<PagedResult<ToDoListDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken token = default)
        {
            var toDoLists = await _unitOfWork.ToDoListRepository.GetAllAsync(paginationRequest.Page, paginationRequest.PageSize, token);
            return _mapper.Map<PagedResult<ToDoListDto>>(toDoLists);
        }

        public async Task<ToDoListDto?> GetByIdAsync(int id, CancellationToken token = default)
        {
            var entity = await _unitOfWork.ToDoListRepository.GetByIdAsync(id, token);
            return entity is not null ? _mapper.Map<ToDoListDto>(entity) : null;
        }

        public async Task<ToDoListDto> UpdateAsync(UpdateToDoListDto updateToDoListDto, CancellationToken token = default)
        {
            if (await _unitOfWork.ToDoListRepository.GetByIdAsync(updateToDoListDto.Id, token) is not ToDoListEntity entity)
            {
                throw new NotFoundException($"To-do list with ID '{updateToDoListDto.Id}' not found.");
            }

            _mapper.Map(updateToDoListDto, entity);
            _unitOfWork.ToDoListRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(token);
            return _mapper.Map<ToDoListDto>(entity);
        }
    }
}
