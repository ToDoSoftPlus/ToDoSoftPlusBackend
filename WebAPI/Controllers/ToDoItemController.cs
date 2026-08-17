using Application.DTOs.ToDoItem;
using Application.Interfaces.Services.EF;
using Application.Interfaces.Services.Validation;
using Application.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/todo-items")]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService _toDoItemService;
        private readonly IValidationService _validationService;

        public ToDoItemController(IToDoItemService toDoItemService, IValidationService validationService)
        {
            _toDoItemService = toDoItemService;
            _validationService = validationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateToDoItemDto dto, CancellationToken cancellationToken)
        {
            await _validationService.ValidateAsync(dto, cancellationToken);

            var toDoItem = await _toDoItemService.AddAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = toDoItem.Id });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UpdateToDoItemDto dto, CancellationToken cancellationToken)
        {
            await _validationService.ValidateAsync(dto, cancellationToken);

            var toDoItem = await _toDoItemService.UpdateAsync(dto, cancellationToken);
            return Ok(toDoItem);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _toDoItemService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var toDoItem = await _toDoItemService.GetByIdAsync(id, cancellationToken);
            return Ok(toDoItem);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            await _validationService.ValidateAsync(paginationRequest, cancellationToken);

            var toDoItems = await _toDoItemService.GetAllAsync(paginationRequest, cancellationToken);
            return Ok(toDoItems);
        }
    }
}
