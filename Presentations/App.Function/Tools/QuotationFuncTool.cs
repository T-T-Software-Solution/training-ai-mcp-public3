using App.AppCore.Interfaces;
using App.AppCore.Models;
using App.AppCore.ToolInformations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Function.Tools;

public class QuotationFuncTool
{
    private readonly IQuotationService _quotationService;
    private readonly ILogger<QuotationFuncTool> _logger;

    public QuotationFuncTool(IQuotationService quotationService, ILogger<QuotationFuncTool> logger)
    {
        _quotationService = quotationService;
        _logger = logger;
    }

    [Function(nameof(GetQuotationById))]
    public async Task<Quotation?> GetQuotationById(
        [McpToolTrigger("GetQuotationById", "Get quotation details by quotation ID. / ดึงข้อมูลใบเสนอราคาด้วยรหัสใบเสนอราคา")] ToolInvocationContext context,
        [McpToolProperty("id", "string", "Quotation ID (send as Guid string) / รหัสใบเสนอราคา (ต้องส่งเป็น Guid string)")] string id,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(id, out var guidId))
            return null;
        return await _quotationService.GetQuotationByIdAsync(guidId);
    }

    [Function(nameof(GetQuotationsByCustomerId))]
    public async Task<IEnumerable<Quotation>> GetQuotationsByCustomerId(
        [McpToolTrigger("GetQuotationsByCustomerId", "Get quotations by customer ID. / ดึงใบเสนอราคาทั้งหมดของลูกค้า")] ToolInvocationContext context,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(customerId, out var guidCustomerId))
            return Enumerable.Empty<Quotation>();
        return await _quotationService.GetQuotationsByCustomerIdAsync(guidCustomerId);
    }

    [Function(nameof(CreateQuotation))]
    public async Task CreateQuotation(
        [McpToolTrigger("CreateQuotation", "Create a new quotation. / สร้างใบเสนอราคาใหม่")] ToolInvocationContext context,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("vehicleModel", "string", "Vehicle model / รุ่นรถ")] string? vehicleModel,
        [McpToolProperty("vehicleGrade", "string", "Vehicle grade / เกรดรถ")] string? vehicleGrade,
        [McpToolProperty("price", "string", "Quotation price (send as decimal string, e.g. 12345.67) / ราคาเสนอขาย (ต้องส่งเป็นสตริงทศนิยม)")] string price,
        [McpToolProperty("discount", "string", "Discount (send as decimal string, e.g. 1000.00) / ส่วนลด (ต้องส่งเป็นสตริงทศนิยม)")] string discount,
        [McpToolProperty("status", "string", "Quotation status / สถานะใบเสนอราคา")] string? status,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(customerId, out var guidCustomerId))
            return;
        if (!decimal.TryParse(price, out var parsedPrice))
            return;
        if (!decimal.TryParse(discount, out var parsedDiscount))
            return;

        var quotation = new Quotation
        {
            Id = Guid.NewGuid(),
            CustomerId = guidCustomerId,
            VehicleModel = vehicleModel,
            VehicleGrade = vehicleGrade,
            Price = parsedPrice,
            Discount = parsedDiscount,
            Status = status,
            CreatedDate = DateTime.UtcNow
        };
        await _quotationService.CreateQuotationAsync(quotation);
        _logger.LogInformation($"Created quotation for customer {customerId}");
    }
}
