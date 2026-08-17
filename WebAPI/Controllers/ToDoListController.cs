using Application.DTOs.ToDoList;
using Application.Interfaces.Services.EF;
using Application.Interfaces.Services.Validation;
using Application.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/todo-lists")]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListService _toDoListService;
        private readonly IValidationService _validationService;

        public ToDoListController(IToDoListService toDoListService, IValidationService validationService)
        {
            _toDoListService = toDoListService;
            _validationService = validationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateToDoListDto dto, CancellationToken cancellationToken)
        {
            await _validationService.ValidateAsync(dto, cancellationToken);

            var toDoList = await _toDoListService.AddAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = toDoList.Id });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UpdateToDoListDto dto, CancellationToken cancellationToken)
        {
            await _validationService.ValidateAsync(dto, cancellationToken);

            var toDoList = await _toDoListService.UpdateAsync(dto, cancellationToken);
            return Ok(toDoList);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _toDoListService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var toDoList = await _toDoListService.GetByIdAsync(id, cancellationToken);
            return Ok(toDoList);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            await _validationService.ValidateAsync(paginationRequest, cancellationToken);

            var toDoLists = await _toDoListService.GetAllAsync(paginationRequest, cancellationToken);
            return Ok(toDoLists);
        }
    }
}
