using FleetMaintenanceApi.DTOs;
using FleetMaintenanceApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetMaintenanceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _service;

        public MaintenanceController(
            IMaintenanceService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            MaintenanceCreateDto dto)
        {
            var result =
                await _service.AddMaintenanceAsync(dto);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery]
            MaintenanceFilterRequestDto filter)
        {
            var result =
                await _service
                .GetMaintenanceRecordsAsync(filter);

            return Ok(result);
        }
    }
}