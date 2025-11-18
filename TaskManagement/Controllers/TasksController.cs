using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Api.DTOs.Tasks;
using TaskManagement.Api.Services.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private long GetUserId()
        {
            return long.Parse(User.FindFirstValue("UserId")!);
        }

        [HttpPost("GetByCriteria")]
        public async Task<IActionResult> GetByCriteria([FromBody] TaskFilterDto filter)
        {
            var userId = GetUserId();
            var result = await _taskService.GetByCriteriaAsync(userId, filter);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var userId = GetUserId();
            var result = await _taskService.GetByIdAsync(id, userId);

            if (result == null)
                return BadRequest(new { Error = "Task not found or unauthorized." });

            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] TaskCreateDto dto)
        {
            try
            {
                var userId = GetUserId();
                await _taskService.AddAsync(userId, dto);
                return Ok(new { Message = "Task added successfully." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TaskUpdateDto dto)
        {
            try
            {
                var userId = GetUserId();
                await _taskService.UpdateAsync(userId, dto);
                return Ok(new { Message = "Task updated successfully." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var userId = GetUserId();
                await _taskService.DeleteAsync(id, userId);
                return Ok(new { Message = "Task deleted successfully." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
