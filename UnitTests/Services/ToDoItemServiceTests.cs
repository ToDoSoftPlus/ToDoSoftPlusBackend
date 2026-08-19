using Application.DTOs.ToDoItem;
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
    public class ToDoItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IToDoItemRepository> _repositoryMock;
        private readonly Mock<IToDoListRepository> _toDoListRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        private readonly ToDoItemService _service;

        private const int CurrentUserId = 1;

        public ToDoItemServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IToDoItemRepository>();
            _toDoListRepositoryMock = new Mock<IToDoListRepository>();
            _mapperMock = new Mock<IMapper>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(CurrentUserId);

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository)
                .Returns(_repositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository)
                .Returns(_toDoListRepositoryMock.Object);

            _service = new ToDoItemService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowNotFoundException_WhenToDoListDoesNotExist()
        {
            var createDto = new CreateToDoItemDto { ToDoListId = 999, Title = "Test Item" };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, createDto.ToDoListId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoListEntity?)null);

            Func<Task> act = async () => await _service.AddAsync(createDto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddAsync_ShouldAddToDoItem_WhenToDoListExists()
        {
            var createDto = new CreateToDoItemDto { ToDoListId = 1, Title = "Test Item", Description = "Test Description" };

            var toDoListEntity = new ToDoListEntity { Id = 1, UserId = CurrentUserId };

            var toDoItemEntity = new ToDoItemEntity { Id = 1, Title = createDto.Title, Description = createDto.Description, ToDoListId = createDto.ToDoListId };

            var excpectedDto = new ToDoItemDto { Id = toDoItemEntity.Id, Title = toDoItemEntity.Title, Description = toDoItemEntity.Description };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, createDto.ToDoListId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoListEntity);

            _mapperMock
                .Setup(x => x.Map<ToDoItemEntity>(createDto))
                .Returns(toDoItemEntity);

            _mapperMock
                .Setup(x => x.Map<ToDoItemDto>(toDoItemEntity))
                .Returns(excpectedDto);

            var result = await _service.AddAsync(createDto);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(excpectedDto);

            _unitOfWorkMock.Verify(x => x.ToDoItemRepository.Add(toDoItemEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenToDoItemDoesNotExist()
        {
            int toDoItemId = 999;

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetByIdAsync(CurrentUserId, toDoItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoItemEntity?)null);

            Func<Task> act = async () => await _service.DeleteAsync(toDoItemId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteToDoItem_WhenToDoItemExists()
        {
            int toDoItemId = 1;

            var toDoItemEntity = new ToDoItemEntity { Id = toDoItemId, Title = "Test Item", Description = "Test Description", ToDoListId = 1 };

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetByIdAsync(CurrentUserId, toDoItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoItemEntity);

            await _service.DeleteAsync(toDoItemId);

            _unitOfWorkMock.Verify(x => x.ToDoItemRepository.Delete(toDoItemEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenToDoItemDoesNotExist()
        {
            int toDoItemId = 999;

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetByIdAsync(CurrentUserId, toDoItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoItemEntity?)null);

            Func<Task> act = async () => await _service.GetByIdAsync(toDoItemId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnToDoItemDto_WhenToDoItemExists()
        {
            int toDoItemId = 1;

            var toDoItemEntity = new ToDoItemEntity { Id = toDoItemId, Title = "Test Item", Description = "Test Description", ToDoListId = 1 };

            var expectedDto = new ToDoItemDto { Id = toDoItemEntity.Id, Title = toDoItemEntity.Title, Description = toDoItemEntity.Description, ToDoListId = toDoItemEntity.ToDoListId };

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetByIdAsync(CurrentUserId, toDoItemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoItemEntity);

            _mapperMock
                .Setup(x => x.Map<ToDoItemDto>(toDoItemEntity))
                .Returns(expectedDto);

            var result = await _service.GetByIdAsync(toDoItemId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedResultOfToDoItemDto()
        {
            var paginationRequest = new PaginationRequest { Page = 1, PageSize = 10 };

            var toDoItemEntities = new PagedResult<ToDoItemEntity>
            {
                Items = new List<ToDoItemEntity>
                {
                    new ToDoItemEntity { Id = 1, Title = "Test Item 1", Description = "Test Description 1", ToDoListId = 1 },
                    new ToDoItemEntity { Id = 2, Title = "Test Item 2", Description = "Test Description 2", ToDoListId = 1 }
                },
                TotalCount = 2,
                HasNextPage = false,
                HasPreviousPage = false,
                Page = 1,
                PageSize = 10,
                TotalPages = 1
            };

            var expectedDtos = new PagedResult<ToDoItemDto>
            {
                Items = new List<ToDoItemDto>
                {
                    new ToDoItemDto { Id = 1, Title = "Test Item 1", Description = "Test Description 1", ToDoListId = 1 },
                    new ToDoItemDto { Id = 2, Title = "Test Item 2", Description = "Test Description 2", ToDoListId = 1 }
                },
                TotalCount = 2,
                HasNextPage = false,
                HasPreviousPage = false,
                Page = 1,
                PageSize = 10,
                TotalPages = 1
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetAllAsync(CurrentUserId, paginationRequest.Page, paginationRequest.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoItemEntities);

            _mapperMock
                .Setup(x => x.Map<PagedResult<ToDoItemDto>>(toDoItemEntities))
                .Returns(expectedDtos);

            var result = await _service.GetAllAsync(paginationRequest);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDtos);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenToDoItemDoesNotExist()
        {
            var updateDto = new UpdateToDoItemDto { Id = 999, Title = "Updated Item", Description = "Updated Description" };

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetByIdAsync(CurrentUserId, updateDto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoItemEntity?)null);

            Func<Task> act = async () => await _service.UpdateAsync(updateDto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateToDoItem_WhenToDoItemExists()
        {
            var updateDto = new UpdateToDoItemDto { Id = 1, Title = "Updated Item", Description = "Updated Description" };

            var toDoItemEntity = new ToDoItemEntity 
            { 
                Id = updateDto.Id, 
                Title = "Old Item", 
                Description = "Old Description", 
                ToDoListId = 1 
            };

            var expectedDto = new ToDoItemDto 
            { 
                Id = toDoItemEntity.Id, 
                Title = updateDto.Title, 
                Description = updateDto.Description, 
                ToDoListId = toDoItemEntity.ToDoListId 
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoItemRepository.GetByIdAsync(CurrentUserId, updateDto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoItemEntity);

            _mapperMock
                .Setup(x => x.Map(updateDto, toDoItemEntity))
                .Callback(() =>
                {
                    toDoItemEntity.Title = updateDto.Title;
                    toDoItemEntity.Description = updateDto.Description;
                });

            _mapperMock
                .Setup(x => x.Map<ToDoItemDto>(toDoItemEntity))
                .Returns(expectedDto);

            var result = await _service.UpdateAsync(updateDto);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);

            _unitOfWorkMock.Verify(x => x.ToDoItemRepository.Update(toDoItemEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
