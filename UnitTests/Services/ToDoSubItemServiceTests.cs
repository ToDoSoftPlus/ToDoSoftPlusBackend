using Application.DTOs.ToDoSubItem;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Identity;
using Application.Interfaces.UnitOfWork;
using Application.Models.Pagination;
using Application.Services.EF;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.Services
{
    public class ToDoSubItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IToDoItemRepository> _toDoItemRepositoryMock;
        private readonly Mock<IToDoSubItemRepository> _repositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        private readonly ToDoSubItemService _service;

        private const int CurrentUserId = 1;

        public ToDoSubItemServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _toDoItemRepositoryMock = new Mock<IToDoItemRepository>();
            _repositoryMock = new Mock<IToDoSubItemRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(CurrentUserId);

            _unitOfWorkMock
                .Setup(x => x.ToDoSubItemRepository)
                .Returns(_repositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository)
                .Returns(_toDoItemRepositoryMock.Object);

            _service = new ToDoSubItemService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowNotFoundException_WhenItemDoesNotExist()
        {
            var createSubItemDto = new CreateToDoSubItemDto { Description = "Test", ToDoItemId = 999 };

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetByIdAsync(CurrentUserId, createSubItemDto.ToDoItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoItemEntity?)null);

            Func<Task> act = async () => await _service.AddAsync(createSubItemDto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddAsync_ShouldAddToDoSubItem_WhenToDoItemExists()
        {
            var createSubItemDto = new CreateToDoSubItemDto { Description = "Test", ToDoItemId = 1 };

            var toDoItemEntity = new ToDoItemEntity { Id = 1, Title = "Item", ToDoListId = 1, };

            var toDoSubItemEntity = new ToDoSubItemEntity { Id = 1, Description = createSubItemDto.Description, ToDoItemId = createSubItemDto.ToDoItemId };

            var excpectedDto = new ToDoSubItemDto { Id = 1, Description = toDoSubItemEntity.Description, ToDoItemId = toDoSubItemEntity.ToDoItemId };

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetByIdAsync(CurrentUserId, createSubItemDto.ToDoItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoItemEntity);

            _mapperMock
                .Setup(x => x.Map<ToDoSubItemEntity>(createSubItemDto))
                .Returns(toDoSubItemEntity);

            _mapperMock
                .Setup(x => x.Map<ToDoSubItemDto>(toDoSubItemEntity))
                .Returns(excpectedDto);

            var result = await _service.AddAsync(createSubItemDto);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(excpectedDto);

            _repositoryMock.Verify(x => x.Add(toDoSubItemEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenToDoSubItemDoesNotExist()
        {
            var toDoSubItemId = 999;

            _unitOfWorkMock
                .Setup(x => x.ToDoSubItemRepository.GetByIdAsync(CurrentUserId, toDoSubItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoSubItemEntity?)null);

            Func<Task> act = async () => await _service.DeleteAsync(toDoSubItemId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteToDoSubItem_WhenToDoSubItemExist()
        {
            var toDoSubItemEntity = new ToDoSubItemEntity() { Id = 1, Description = "Test" };

            _unitOfWorkMock
                .Setup(x => x.ToDoSubItemRepository.GetByIdAsync(CurrentUserId, toDoSubItemEntity.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoSubItemEntity);

            await _service.DeleteAsync(toDoSubItemEntity.Id);

            _repositoryMock.Verify(x => x.Delete(toDoSubItemEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenToDoSubItemDoesNotExist()
        {
            var toDoSubItemId = 999;

            _unitOfWorkMock
                .Setup(x => x.ToDoSubItemRepository.GetByIdAsync(CurrentUserId, toDoSubItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoSubItemEntity?)null);

            Func<Task> act = async () => await _service.GetByIdAsync(toDoSubItemId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnToDoSubItem_WhenToDoSubItemExist()
        {
            var toDoSubItemId = 1;

            var toDoSubItemEntity = new ToDoSubItemEntity() { Id = toDoSubItemId, Description = "Sub Item" };

            var excpectedDto = new ToDoSubItemDto() { Id = toDoSubItemEntity.Id, Description = toDoSubItemEntity.Description };

            _unitOfWorkMock
                .Setup(x => x.ToDoSubItemRepository.GetByIdAsync(CurrentUserId, toDoSubItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoSubItemEntity);

            _mapperMock
                .Setup(x => x.Map<ToDoSubItemDto>(toDoSubItemEntity))
                .Returns(excpectedDto);

            var result = await _service.GetByIdAsync(toDoSubItemId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(excpectedDto);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedResultOfToDoSubItem()
        {
            var paginationRequest = new PaginationRequest()
            {
                Page = 1,
                PageSize = 10,
            };

            var pagedResultToDoSubItemEntity = new PagedResult<ToDoSubItemEntity>()
            {
                Items = new List<ToDoSubItemEntity>()
                {
                    new ToDoSubItemEntity() { Id = 1, Description = "Sub-item 1"},
                    new ToDoSubItemEntity() { Id = 1, Description = "Sub-item 2"},
                },
                TotalCount = 2,
                HasNextPage = false,
                HasPreviousPage = false,
                Page = 1,
                PageSize = 10,
                TotalPages = 1,
            };

            var excpectedDto = new PagedResult<ToDoSubItemDto>()
            {
                Items = new List<ToDoSubItemDto>()
                {
                    new ToDoSubItemDto() { Id = 1, Description = "Sub-item 1" },
                    new ToDoSubItemDto() { Id = 1, Description = "Sub-item 2" },
                },
                TotalCount = 2,
                HasNextPage = false,
                HasPreviousPage = false,
                Page = 1,
                PageSize = 10,
                TotalPages = 1,
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoSubItemRepository.GetAllAsync(CurrentUserId, paginationRequest.Page, paginationRequest.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResultToDoSubItemEntity);

            _mapperMock
                .Setup(x => x.Map<PagedResult<ToDoSubItemDto>>(pagedResultToDoSubItemEntity))
                .Returns(excpectedDto);

            var result = await _service.GetAllAsync(paginationRequest);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(excpectedDto);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenToDoSubItemDoesNotExist()
        {
            var updateToDoSubItemDto = new UpdateToDoSubItemDto() { Id = 999 };

            _unitOfWorkMock
                .Setup(x => x.ToDoSubItemRepository.GetByIdAsync(CurrentUserId, updateToDoSubItemDto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoSubItemEntity?)null);

            Func<Task> act = async () => await _service.UpdateAsync(updateToDoSubItemDto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateToDoSubItem_WhenToDoSubItemExist()
        {
            var updateToDoSubItemDto = new UpdateToDoSubItemDto() 
            { 
                Id = 1, 
                Description = "Update description", 
                ToDoItemId = 1 
            };

            var toDoSubItemEntity = new ToDoSubItemEntity()
            {
                Id = 1,
                Description = "Old description",
                ToDoItemId = 1
            };

            var excpectedDto = new ToDoSubItemDto()
            {
                Id = 1,
                Description = updateToDoSubItemDto.Description,
                ToDoItemId = 1,
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoSubItemRepository.GetByIdAsync(CurrentUserId, updateToDoSubItemDto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoSubItemEntity);

            _mapperMock
                .Setup(x => x.Map(updateToDoSubItemDto, toDoSubItemEntity))
                .Callback<UpdateToDoSubItemDto, ToDoSubItemEntity>((dto, entity) =>
                {
                    entity.Description = dto.Description;
                });

            _mapperMock
                .Setup(x => x.Map<ToDoSubItemDto>(toDoSubItemEntity))
                .Returns(excpectedDto);

            var result = await _service.UpdateAsync(updateToDoSubItemDto);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(excpectedDto);

            _unitOfWorkMock.Verify(x => x.ToDoSubItemRepository.Update(toDoSubItemEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
