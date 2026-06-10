using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveAPI.Controllers
{
    [Route("api/leaverequests")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService service;

        public LeaveRequestsController(
            ILeaveRequestService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            LeaveRequestCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result =
                await service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.LeaveRequestId },
                result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result =
                await service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>
            GetById(int id)
        {
            var result =
                await service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}