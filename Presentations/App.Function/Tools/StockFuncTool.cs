using App.AppCore.Models;
using App.AppCore.Interfaces;
using App.AppCore.ToolInformations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Function.Tools;

public class StockFuncTool
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockFuncTool> _logger;

    public StockFuncTool(IStockService stockService, ILogger<StockFuncTool> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    [Function(nameof(GetStockItems))]
    public async Task<IEnumerable<StockItem>> GetStockItems(
        [McpToolTrigger("GetStockItems", "Get stock items by model and/or color. / ดึงรายการสต็อกตามรุ่นหรือสี")] ToolInvocationContext context,
        [McpToolProperty("model", "string", "Model (optional) / รุ่น (ไม่ระบุก็ได้)")] string? model,
        [McpToolProperty("color", "string", "Color (optional) / สี (ไม่ระบุก็ได้)")] string? color,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        return await _stockService.GetStockItemsAsync(model, color);
    }

    [Function(nameof(GetStockItemById))]
    public async Task<StockItem?> GetStockItemById(
        [McpToolTrigger("GetStockItemById", "Get stock item by ID. / ดึงรายการสต็อกด้วยรหัสสินค้า (Stock item ID ต้องเป็น Guid)")] ToolInvocationContext context,
        [McpToolProperty("id", "string", "Stock item ID (send as Guid string) / รหัสสินค้า (ต้องส่งเป็น Guid string)")] string id,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(id, out var guidId))
            return null;
        return await _stockService.GetStockItemByIdAsync(guidId);
    }
}
