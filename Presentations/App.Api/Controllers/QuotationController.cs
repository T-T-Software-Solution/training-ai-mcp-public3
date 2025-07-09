using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.AppCore.Interfaces;
using App.AppCore.Models;

namespace App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationService _quotationService;

        public QuotationController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var quotation = await _quotationService.GetQuotationByIdAsync(id);
            if (quotation == null) return NotFound();
            return Ok(quotation);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var quotations = await _quotationService.GetQuotationsByCustomerIdAsync(customerId);
            return Ok(quotations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Quotation quotation)
        {
            await _quotationService.CreateQuotationAsync(quotation);
            return CreatedAtAction(nameof(GetById), new { id = quotation.Id }, quotation);
        }
    }
}
