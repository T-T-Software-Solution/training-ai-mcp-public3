using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.AppCore.Interfaces;
using App.AppCore.Models;

namespace App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? model, [FromQuery] string? color)
        {
            var items = await _stockService.GetStockItemsAsync(model, color);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _stockService.GetStockItemByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
