using App.AppCore.Models;
using App.AppCore.Models.Dtos;
using App.AppCore.Interfaces;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace App.Mcp.Tools;

[McpServerToolType]
public class VehicleTools
{
    private readonly IVehicleService _vehicleService;

    public VehicleTools(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [McpServerTool, Description("Get vehicle details by vehicle ID. / ดึงข้อมูลรถยนต์ด้วยรหัสรถยนต์ (Vehicle ID ต้องเป็น Guid)")]
    [Authorize]
    public async Task<VehicleDto?> GetVehicleById(
        [Description("Vehicle ID (Guid) / รหัสรถยนต์ (ต้องเป็น Guid)")] Guid id)
    {
        var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
        if (vehicle == null) return null;
        return new VehicleDto
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Model = vehicle.Model,
            Color = vehicle.Color,
            CustomerId = vehicle.CustomerId
        };
    }

    [McpServerTool, Description("Get all vehicles for a customer. / ดึงรถยนต์ทั้งหมดของลูกค้า")]
    [Authorize]
    public async Task<IEnumerable<VehicleDto>> GetVehiclesByCustomer(
        [Description("Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId)
    {
        var vehicles = await _vehicleService.GetVehiclesByCustomerIdAsync(customerId);
        return vehicles.Select(vehicle => new VehicleDto
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Model = vehicle.Model,
            Color = vehicle.Color,
            CustomerId = vehicle.CustomerId
        });
    }

    [McpServerTool, Description("Add a new vehicle. / เพิ่มรถยนต์ใหม่")]
    [Authorize]
    public async Task AddVehicle(
        [Description("License plate / ทะเบียนรถยนต์")] string? licensePlate,
        [Description("Model / รุ่นรถยนต์")] string? model,
        [Description("Color / สีรถยนต์")] string? color,
        [Description("Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = licensePlate,
            Model = model,
            Color = color,
            CustomerId = customerId
        };
        await _vehicleService.AddVehicleAsync(vehicle);
    }

    [McpServerTool, Description("Update an existing vehicle. / แก้ไขข้อมูลรถยนต์")]
    [Authorize]
    public async Task UpdateVehicle(
        [Description("Vehicle ID (Guid) / รหัสรถยนต์ (ต้องเป็น Guid)")] Guid id,
        [Description("License plate / ทะเบียนรถยนต์")] string? licensePlate,
        [Description("Model / รุ่นรถยนต์")] string? model,
        [Description("Color / สีรถยนต์")] string? color,
        [Description("Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId)
    {
        var vehicle = new Vehicle
        {
            Id = id,
            LicensePlate = licensePlate,
            Model = model,
            Color = color,
            CustomerId = customerId
        };
        await _vehicleService.UpdateVehicleAsync(vehicle);
    }
}