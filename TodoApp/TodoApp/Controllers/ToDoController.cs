using BusinessLogic.Blas;
using BusinessLogic.Services;
using CodeGen;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TodoApp.DtoToBlaMappers;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController(IToDoService service) : ControllerBase
    {
        [HttpGet("AllTodos")]
        public async Task<List<ToDoResponse>> GetAllTodos([FromQuery] string? description)
        {
            var res = await service.GetAllAsync(description);
            return res.Select(x => new ToDoResponse(x.Id, x.Title, x.Description, x.Status.MapToDto())).ToList();
        }
        [HttpGet("ByStatus")]
        public async Task<List<ToDoResponse>> GetByStatus([FromQuery][Required] StatusEnum status)
        {
            var res = await service.GetAllByStatus(status.MapToBla());
            return res.Select(x => new ToDoResponse(x.Id, x.Title, x.Description, x.Status.MapToDto())).ToList();
        }
        [HttpGet("SingleTodo/{id}")]
        public async Task<ToDoResponse> GetSingleTodo([FromRoute][Required] string id)
        {
            var res = await service.GetAsync(id);
            return new ToDoResponse(res.Id, res.Title, res.Description, res.Status.MapToDto());
        }
        [HttpPost("CreateNewTodo")]
        public async Task<CreateTodoResult> CreateNewTodo([FromBody][Required] ToDoPostRequestBody request)
        {
            var res = await service.CreateNewTodo(new ToDoPostRequestBodyBla(request.Title, request.Description, request.Status.MapToBla()));
            return new CreateTodoResult(res);
        }
        [HttpDelete("DeleteTodo/{id}")]
        public async Task<IActionResult> DeleteTodo([FromRoute][Required] string id)
        {
            await service.DeleteAsync(id);
            return new NoContentResult();
        }
        [HttpPatch("UpdateTodo/{id}")]
        public async Task<IActionResult> UpdateTodo([FromRoute][Required] string id, [FromBody][Required] ToDoChangeRequest request)
        {
            if (request.Status is null && request.Description is null)
            {
                return new BadRequestResult();
            }
            await service.UpdateAsync(new ToDoChangeRequestBla(id, request.Status?.MapToBla(), request.Description));
            return new NoContentResult();
        }
        [HttpGet("GetTodosByPage")]
        public async Task<ToDoByPageResponse> GetTodosByPage([FromQuery] string? continuationToken, [FromQuery][Range(1, 25, ErrorMessage = "Value must be between 1 and 25.")] int pageSize)
        {
            var res = await service.ListByPageAsync(continuationToken, pageSize);
            return new ToDoByPageResponse(res.ContinuationToken, [..res.Results.Select
                (x => new ToDoResponse(x.Id, x.Title, x.Description, x.Status.MapToDto()))]);
        }
    }
}
