using Application.DTOs.ToDoItem;
using Application.Interfaces.Services;
using Application.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/todo-items")]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService _toDoItemService;

        public ToDoItemController(IToDoItemService toDoItemService)
        {
            _toDoItemService = toDoItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateToDoItemDto dto, CancellationToken cancellationToken)
        {
            var toDoItem = await _toDoItemService.AddAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(CreateToDoItemDto), new { id = toDoItem.Id }, toDoItem);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateToDoItemDto dto, CancellationToken cancellationToken)
        {
            var toDoItem = await _toDoItemService.UpdateAsync(dto, cancellationToken);
            return Ok(toDoItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _toDoItemService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var toDoItem = await _toDoItemService.GetByIdAsync(id, cancellationToken);
            return Ok(toDoItem);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var toDoItems = await _toDoItemService.GetAllAsync(paginationRequest, cancellationToken);
            return Ok(toDoItems);
        }
    }
}
