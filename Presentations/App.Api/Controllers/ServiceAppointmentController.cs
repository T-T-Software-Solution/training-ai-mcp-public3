using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.AppCore.Interfaces;
using App.AppCore.Models;

namespace App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceAppointmentController : ControllerBase
    {
        private readonly IServiceAppointmentService _serviceAppointmentService;

        public ServiceAppointmentController(IServiceAppointmentService serviceAppointmentService)
        {
            _serviceAppointmentService = serviceAppointmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var appointment = await _serviceAppointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var appointments = await _serviceAppointmentService.GetAppointmentsByCustomerIdAsync(customerId);
            return Ok(appointments);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSlots(DateTime date, string serviceType)
        {
            var slots = await _serviceAppointmentService.GetAvailableSlotsAsync(date, serviceType);
            return Ok(slots);
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] ServiceAppointment appointment)
        {
            await _serviceAppointmentService.BookAppointmentAsync(appointment);
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            await _serviceAppointmentService.UpdateAppointmentStatusAsync(id, status);
            return NoContent();
        }
    }
}
