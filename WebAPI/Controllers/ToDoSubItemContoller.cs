using Application.DTOs.ToDoSubItem;
using Application.Interfaces.Services.EF;
using Application.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/todo-sub-items")]
    public class ToDoSubItemContoller : ControllerBase
    {
        private readonly IToDoSubItemService _toDoSubItemService;
        public ToDoSubItemContoller(IToDoSubItemService toDoSubItemService)
        {
            _toDoSubItemService = toDoSubItemService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateToDoSubItemDto dto, CancellationToken cancellationToken)
        {
            var toDoSubItem = await _toDoSubItemService.AddAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = toDoSubItem.Id });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UpdateToDoSubItemDto dto, CancellationToken cancellationToken)
        {
            var toDoSubItem = await _toDoSubItemService.UpdateAsync(dto, cancellationToken);
            return Ok(toDoSubItem);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _toDoSubItemService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var toDoSubItem = await _toDoSubItemService.GetByIdAsync(id, cancellationToken);
            return Ok(toDoSubItem);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var toDoSubItems = await _toDoSubItemService.GetAllAsync(paginationRequest, cancellationToken);
            return Ok(toDoSubItems);
        }
    }
}
