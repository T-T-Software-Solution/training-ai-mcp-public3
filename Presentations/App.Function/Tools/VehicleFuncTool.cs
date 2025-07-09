using App.AppCore.Models;
using App.AppCore.Models.Dtos;
using App.AppCore.Interfaces;
using App.AppCore.ToolInformations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Function.Tools;

public class VehicleFuncTool
{
    private readonly IVehicleService _vehicleService;
    private readonly ILogger<VehicleFuncTool> _logger;

    public VehicleFuncTool(IVehicleService vehicleService, ILogger<VehicleFuncTool> logger)
    {
        _vehicleService = vehicleService;
        _logger = logger;
    }

    [Function(nameof(GetVehicleById))]
    public async Task<VehicleDto?> GetVehicleById(
        [McpToolTrigger("GetVehicleById", "Get vehicle details by vehicle ID. / ดึงข้อมูลรถยนต์ด้วยรหัสรถยนต์ (Vehicle ID ต้องเป็น Guid)")] ToolInvocationContext context,
        [McpToolProperty("id", "string", "Vehicle ID (send as Guid string) / รหัสรถยนต์ (ต้องส่งเป็น Guid string)")] string id,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(id, out var guidId))
            return null;
        var vehicle = await _vehicleService.GetVehicleByIdAsync(guidId);
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

    [Function(nameof(GetVehiclesByCustomer))]
    public async Task<IEnumerable<VehicleDto>> GetVehiclesByCustomer(
        [McpToolTrigger("GetVehiclesByCustomer", "Get all vehicles for a customer. / ดึงรถยนต์ทั้งหมดของลูกค้า")] ToolInvocationContext context,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(customerId, out var guidCustomerId))
            return Enumerable.Empty<VehicleDto>();
        var vehicles = await _vehicleService.GetVehiclesByCustomerIdAsync(guidCustomerId);
        return vehicles.Select(vehicle => new VehicleDto
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Model = vehicle.Model,
            Color = vehicle.Color,
            CustomerId = vehicle.CustomerId
        });
    }

    [Function(nameof(AddVehicle))]
    public async Task AddVehicle(
        [McpToolTrigger("AddVehicle", "Add a new vehicle. / เพิ่มรถยนต์ใหม่")] ToolInvocationContext context,
        [McpToolProperty("licensePlate", "string", "License plate / ทะเบียนรถยนต์")] string? licensePlate,
        [McpToolProperty("model", "string", "Model / รุ่นรถยนต์")] string? model,
        [McpToolProperty("color", "string", "Color / สีรถยนต์")] string? color,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(customerId, out var guidCustomerId))
            return;
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = licensePlate,
            Model = model,
            Color = color,
            CustomerId = guidCustomerId
        };
        await _vehicleService.AddVehicleAsync(vehicle);
    }

    [Function(nameof(UpdateVehicle))]
    public async Task UpdateVehicle(
        [McpToolTrigger("UpdateVehicle", "Update an existing vehicle. / แก้ไขข้อมูลรถยนต์")] ToolInvocationContext context,
        [McpToolProperty("id", "string", "Vehicle ID (send as Guid string) / รหัสรถยนต์ (ต้องส่งเป็น Guid string)")] string id,
        [McpToolProperty("licensePlate", "string", "License plate / ทะเบียนรถยนต์")] string? licensePlate,
        [McpToolProperty("model", "string", "Model / รุ่นรถยนต์")] string? model,
        [McpToolProperty("color", "string", "Color / สีรถยนต์")] string? color,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(id, out var guidId) || !Guid.TryParse(customerId, out var guidCustomerId))
            return;
        var vehicle = new Vehicle
        {
            Id = guidId,
            LicensePlate = licensePlate,
            Model = model,
            Color = color,
            CustomerId = guidCustomerId
        };
        await _vehicleService.UpdateVehicleAsync(vehicle);
    }
}
