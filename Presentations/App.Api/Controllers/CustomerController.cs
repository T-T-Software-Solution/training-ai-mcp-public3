using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.AppCore.Interfaces;
using App.AppCore.Models;

namespace App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            await _customerService.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Customer customer)
        {
            if (id != customer.Id) return BadRequest();
            await _customerService.UpdateCustomerAsync(customer);
            return NoContent();
        }
    }
}
