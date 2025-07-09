using App.AppCore.Models;
using App.AppCore.Interfaces;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace App.Mcp.Tools;

[McpServerToolType]
public class StockTools
{
    private readonly IStockService _stockService;

    public StockTools(IStockService stockService)
    {
        _stockService = stockService;
    }

    [McpServerTool, Description("Get stock items by model and/or color. / ดึงรายการสต็อกตามรุ่นหรือสี")]
    [Authorize]
    public async Task<IEnumerable<StockItem>> GetStockItems(
        [Description("Model (optional) / รุ่น (ไม่ระบุก็ได้)")] string? model,
        [Description("Color (optional) / สี (ไม่ระบุก็ได้)")] string? color)
    {
        return await _stockService.GetStockItemsAsync(model, color);
    }

    [McpServerTool, Description("Get stock item by ID. / ดึงรายการสต็อกด้วยรหัสสินค้า (Stock item ID ต้องเป็น Guid)")]
    [Authorize]
    public async Task<StockItem?> GetStockItemById(
        [Description("Stock item ID (Guid) / รหัสสินค้า (ต้องเป็น Guid)")] Guid id)
    {
        return await _stockService.GetStockItemByIdAsync(id);
    }
}