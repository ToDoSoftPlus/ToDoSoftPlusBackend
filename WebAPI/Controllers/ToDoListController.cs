using Application.DTOs.ToDoList;
using Application.Interfaces.Services;
using Application.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/todo-lists")]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListService _toDoListService;

        public ToDoListController(IToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateToDoListDto dto, CancellationToken cancellationToken)
        {
            var toDoList = await _toDoListService.AddAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(CreateToDoListDto), new { id = toDoList.Id }, toDoList);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateToDoListDto dto, CancellationToken cancellationToken)
        {
            var toDoList = await _toDoListService.UpdateAsync(dto, cancellationToken);

            return Ok(toDoList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _toDoListService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var toDoList = await _toDoListService.GetByIdAsync(id, cancellationToken);
            return Ok(toDoList);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var toDoLists = await _toDoListService.GetAllAsync(paginationRequest, cancellationToken);
            return Ok(toDoLists);
        }
    }
}
