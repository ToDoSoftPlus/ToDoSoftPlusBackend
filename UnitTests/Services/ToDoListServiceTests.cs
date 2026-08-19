using Application.DTOs.ToDoList;
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
    public class ToDoListServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IToDoListRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;

        private readonly ToDoListService _service;

        private const int CurrentUserId = 1;

        public ToDoListServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IToDoListRepository>();
            _mapperMock = new Mock<IMapper>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(CurrentUserId);

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository)
                .Returns(_repositoryMock.Object);

            _service = new ToDoListService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowAlreadyExistsException_WhenListWithSameTitleExists()
        {
            var createToDoListDto = new CreateToDoListDto { Title = "Existing Title" };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.IsExistsByTitleAndUserIdAsync(createToDoListDto.Title, CurrentUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _service.AddAsync(createToDoListDto);

            await act.Should().ThrowAsync<AlreadyExistsException>();
        }

        [Fact]
        public async Task AddAsync_ShouldAddToDoList_WhenListWithSameTitleDoesNotExist()
        {
            var createToDoListDto = new CreateToDoListDto { Title = "New Title", Description = "New Description" };

            var toDoListEntity = new ToDoListEntity
            {
                Id = 10,
                Title = createToDoListDto.Title,
                Description = createToDoListDto.Description,
                UserId = CurrentUserId
            };

            var excpectedDto = new ToDoListDto
            {
                Id = 10,
                Title = "New Title",
                Description = "New Description",
                UserId = toDoListEntity.UserId
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.IsExistsByTitleAndUserIdAsync(createToDoListDto.Title, CurrentUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map<ToDoListEntity>(createToDoListDto))
                .Returns(toDoListEntity);

            _mapperMock
                .Setup(x => x.Map<ToDoListDto>(toDoListEntity))
                .Returns(excpectedDto);

            var result = await _service.AddAsync(createToDoListDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(excpectedDto);

            _unitOfWorkMock.Verify(x => x.ToDoListRepository.Add(It.IsAny<ToDoListEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenListDoesNotExist()
        {
            int listId = 1;

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, listId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoListEntity?)null);

            Func<Task> act = async () => await _service.DeleteAsync(listId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteToDoList_WhenListExists()
        {
            int listId = 1;
            var toDoListEntity = new ToDoListEntity
            {
                Id = listId,
                Title = "Title",
                Description = "Description",
                UserId = CurrentUserId
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, listId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoListEntity);

            await _service.DeleteAsync(listId, CancellationToken.None);

            _unitOfWorkMock.Verify(x => x.ToDoListRepository.Delete(toDoListEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenListDoesNotExist()
        {
            int listId = 1;

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, listId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoListEntity?)null);

            Func<Task> act = async () => await _service.GetByIdAsync(listId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnToDoList_WhenListExists()
        {
            int listId = 1;

            var toDoListEntity = new ToDoListEntity
            {
                Id = listId,
                Title = "Title",
                Description = "Description",
                UserId = CurrentUserId
            };

            var expectedDto = new ToDoListDto
            {
                Id = listId,
                Title = "Title",
                Description = "Description",
                UserId = CurrentUserId
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, listId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toDoListEntity);

            _mapperMock
                .Setup(x => x.Map<ToDoListDto>(toDoListEntity))
                .Returns(expectedDto);

            var result = await _service.GetByIdAsync(listId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedResult()
        {
            var paginationRequest = new PaginationRequest { Page = 1, PageSize = 10 };

            var pagedResultEntity = new PagedResult<ToDoListEntity>
            {
                Items = new List<ToDoListEntity>
                {
                    new ToDoListEntity { Id = 1, Title = "Title1", Description = "Description1", UserId = CurrentUserId },
                    new ToDoListEntity { Id = 2, Title = "Title2", Description = "Description2", UserId = CurrentUserId }
                },
                TotalCount = 2,
                HasNextPage = false,
                HasPreviousPage = false,
                Page = 1,
                PageSize = 10,
                TotalPages = 1,
            };

            var expectedDtoResult = new PagedResult<ToDoListDto>
            {
                Items = new List<ToDoListDto>
                {
                    new ToDoListDto { Id = 1, Title = "Title1", Description = "Description1", UserId = CurrentUserId },
                    new ToDoListDto { Id = 2, Title = "Title2", Description = "Description2", UserId = CurrentUserId }
                },
                TotalCount = 2,
                HasNextPage = false,
                HasPreviousPage = false,
                Page = 1,
                PageSize = 10,
                TotalPages = 1
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetAllAsync(CurrentUserId, paginationRequest.Page, paginationRequest.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResultEntity);

            _mapperMock
                .Setup(x => x.Map<PagedResult<ToDoListDto>>(pagedResultEntity))
                .Returns(expectedDtoResult);

            var result = await _service.GetAllAsync(paginationRequest, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDtoResult);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenListDoesNotExist()
        {
            var updateToDoListDto = new UpdateToDoListDto { Id = 1, Title = "Updated Title", Description = "Updated Description" };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, updateToDoListDto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ToDoListEntity?)null);

            Func<Task> act = async () => await _service.UpdateAsync(updateToDoListDto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowAlreadyExistsException_WhenListWithSameTitleExists()
        {
            var updateToDoListDto = new UpdateToDoListDto { Id = 1, Title = "Existing Title", Description = "Updated Description" };

            var existingEntity = new ToDoListEntity
            {
                Id = 1,
                Title = "Old Title",
                Description = "Old Description",
                UserId = CurrentUserId
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, updateToDoListDto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEntity);

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.IsExistsByTitleAndUserIdAsync(updateToDoListDto.Title, CurrentUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _service.UpdateAsync(updateToDoListDto);

            await act.Should().ThrowAsync<AlreadyExistsException>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateToDoList_WhenListExistsAndTitleIsUnique()
        {
            var updateToDoListDto = new UpdateToDoListDto { Id = 1, Title = "Updated Title", Description = "Updated Description" };

            var existingEntity = new ToDoListEntity
            {
                Id = 1,
                Title = "Old Title",
                Description = "Old Description",
                UserId = CurrentUserId
            };

            var expectedDto = new ToDoListDto
            {
                Id = 1,
                Title = "Updated Title",
                Description = "Updated Description",
                UserId = CurrentUserId
            };

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.GetByIdAsync(CurrentUserId, updateToDoListDto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEntity);

            _unitOfWorkMock
                .Setup(x => x.ToDoListRepository.IsExistsByTitleAndUserIdAsync(updateToDoListDto.Title, CurrentUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map(updateToDoListDto, existingEntity))
                .Callback<UpdateToDoListDto, ToDoListEntity>((dto, entity) =>
                {
                    entity.Title = dto.Title;
                    entity.Description = dto.Description;
                });

            _mapperMock
                .Setup(x => x.Map<ToDoListDto>(existingEntity))
                .Returns(expectedDto);

            var result = await _service.UpdateAsync(updateToDoListDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);

            _unitOfWorkMock.Verify(x => x.ToDoListRepository.Update(existingEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
