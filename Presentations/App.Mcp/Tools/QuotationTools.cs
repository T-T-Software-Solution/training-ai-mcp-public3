using App.AppCore.Models;
using App.AppCore.Interfaces;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace App.Mcp.Tools;

[McpServerToolType]
public class QuotationTools
{
    private readonly IQuotationService _quotationService;

    public QuotationTools(IQuotationService quotationService)
    {
        _quotationService = quotationService;
    }

    [McpServerTool, Description("Get quotation details by quotation ID. / ดึงข้อมูลใบเสนอราคาด้วยรหัสใบเสนอราคา (Quotation ID ต้องเป็น Guid)")]
    [Authorize]
    public async Task<Quotation?> GetQuotationById(
        [Description("Quotation ID (Guid) / รหัสใบเสนอราคา (ต้องเป็น Guid)")] Guid id)
    {
        return await _quotationService.GetQuotationByIdAsync(id);
    }

    [McpServerTool, Description("Get all quotations for a customer. / ดึงใบเสนอราคาทั้งหมดของลูกค้า")]
    [Authorize]
    public async Task<IEnumerable<Quotation>> GetQuotationsByCustomer(
        [Description("Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId)
    {
        return await _quotationService.GetQuotationsByCustomerIdAsync(customerId);
    }

    [McpServerTool, Description("Create a new quotation. / สร้างใบเสนอราคาใหม่")]
    [Authorize]
    public async Task CreateQuotation(
        [Description("Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId,
        [Description("Vehicle model / รุ่นรถยนต์")] string? vehicleModel,
        [Description("Vehicle grade / เกรดรถยนต์")] string? vehicleGrade,
        [Description("Price / ราคา")] decimal price,
        [Description("Discount / ส่วนลด")] decimal discount,
        [Description("Status / สถานะ")] string? status)
    {
        var quotation = new Quotation
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            VehicleModel = vehicleModel,
            VehicleGrade = vehicleGrade,
            Price = price,
            Discount = discount,
            Status = status,
            CreatedDate = DateTime.UtcNow
        };
        await _quotationService.CreateQuotationAsync(quotation);
    }
}