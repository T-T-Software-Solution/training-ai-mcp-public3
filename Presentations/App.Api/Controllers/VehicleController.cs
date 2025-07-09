using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.AppCore.Interfaces;
using App.AppCore.Models;

namespace App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null) return NotFound();
            return Ok(vehicle);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var vehicles = await _vehicleService.GetVehiclesByCustomerIdAsync(customerId);
            return Ok(vehicles);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? model, [FromQuery] string? color)
        {
            var vehicles = await _vehicleService.SearchVehiclesAsync(model, color);
            return Ok(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vehicle vehicle)
        {
            await _vehicleService.AddVehicleAsync(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }
    }
}
