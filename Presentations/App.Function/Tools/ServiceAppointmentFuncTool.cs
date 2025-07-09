using App.AppCore.Interfaces;
using App.AppCore.Models.Dtos;
using App.AppCore.Models;
using App.AppCore.ToolInformations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Function.Tools;

public class ServiceAppointmentFuncTool
{
    private readonly IServiceAppointmentService _serviceAppointmentService;
    private readonly ILogger<ServiceAppointmentFuncTool> _logger;

    public ServiceAppointmentFuncTool(IServiceAppointmentService serviceAppointmentService, ILogger<ServiceAppointmentFuncTool> logger)
    {
        _serviceAppointmentService = serviceAppointmentService;
        _logger = logger;
    }

    [Function(nameof(GetAppointmentById))]
    public async Task<ServiceAppointmentDto?> GetAppointmentById(
        [McpToolTrigger("GetAppointmentById", "Get service appointment details by appointment ID. / ดึงข้อมูลการนัดหมายด้วยรหัสการนัดหมาย (Appointment ID ต้องเป็น Guid)")] ToolInvocationContext context,
        [McpToolProperty("id", "string", "Appointment ID (send as Guid string) / รหัสการนัดหมาย (ต้องส่งเป็น Guid string)")] string id,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(id, out var guidId))
            return null;
        var appointment = await _serviceAppointmentService.GetAppointmentByIdAsync(guidId);
        if (appointment == null) return null;
        return new ServiceAppointmentDto
        {
            Id = appointment.Id,
            AppointmentDate = appointment.AppointmentDate,
            ServiceType = appointment.ServiceType,
            Status = appointment.Status,
            CustomerId = appointment.CustomerId,
            VehicleId = appointment.VehicleId,
            TechnicianId = appointment.TechnicianId
        };
    }

    [Function(nameof(GetAppointmentsByCustomer))]
    public async Task<IEnumerable<ServiceAppointmentDto>> GetAppointmentsByCustomer(
        [McpToolTrigger("GetAppointmentsByCustomer", "Get all service appointments for a customer. / ดึงการนัดหมายทั้งหมดของลูกค้า")] ToolInvocationContext context,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(customerId, out var guidCustomerId))
            return Enumerable.Empty<ServiceAppointmentDto>();
        var appointments = await _serviceAppointmentService.GetAppointmentsByCustomerIdAsync(guidCustomerId);
        return appointments.Select(a => new ServiceAppointmentDto
        {
            Id = a.Id,
            AppointmentDate = a.AppointmentDate,
            ServiceType = a.ServiceType,
            Status = a.Status,
            CustomerId = a.CustomerId,
            VehicleId = a.VehicleId,
            TechnicianId = a.TechnicianId
        });
    }

    [Function(nameof(GetAvailableSlots))]
    public async Task<IEnumerable<ServiceAppointmentDto>> GetAvailableSlots(
        [McpToolTrigger("GetAvailableSlots", "Get available service appointment slots for a date and service type. / ดึงช่วงเวลานัดหมายที่ว่างสำหรับวันที่และประเภทบริการ")] ToolInvocationContext context,
        [McpToolProperty("date", "string", "Date (send as ISO 8601 string, e.g. 2025-07-08 or 2025-07-08T14:00:00) / วันที่ (ต้องส่งเป็นสตริงรูปแบบ ISO 8601)")] string date,
        [McpToolProperty("serviceType", "string", "Service type / ประเภทบริการ")] string serviceType,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
            return Enumerable.Empty<ServiceAppointmentDto>();
        var slots = await _serviceAppointmentService.GetAvailableSlotsAsync(parsedDate, serviceType);
        return slots.Select(a => new ServiceAppointmentDto
        {
            Id = a.Id,
            AppointmentDate = a.AppointmentDate,
            ServiceType = a.ServiceType,
            Status = a.Status,
            CustomerId = a.CustomerId,
            VehicleId = a.VehicleId,
            TechnicianId = a.TechnicianId
        });
    }

    [Function(nameof(BookAppointment))]
    public async Task BookAppointment(
        [McpToolTrigger("BookAppointment", "Book a new service appointment. / จองการนัดหมายบริการใหม่")] ToolInvocationContext context,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("vehicleId", "string", "Vehicle ID (send as Guid string) / รหัสรถยนต์ (ต้องส่งเป็น Guid string)")] string vehicleId,
        [McpToolProperty("appointmentDate", "string", "Appointment date and time (send as ISO 8601 string, e.g. 2025-07-08T14:00:00) / วันและเวลานัดหมาย (ต้องส่งเป็นสตริงรูปแบบ ISO 8601)")] string appointmentDate,
        [McpToolProperty("serviceType", "string", "Service type / ประเภทบริการ")] string serviceType,
        [McpToolProperty("status", "string", "Status / สถานะ")] string? status = null,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(customerId, out var guidCustomerId) || !Guid.TryParse(vehicleId, out var guidVehicleId))
            return;
        if (!DateTime.TryParse(appointmentDate, out var parsedAppointmentDate))
            return;
        var appointment = new ServiceAppointment
        {
            Id = Guid.NewGuid(),
            CustomerId = guidCustomerId,
            VehicleId = guidVehicleId,
            TechnicianId = null,
            AppointmentDate = parsedAppointmentDate,
            ServiceType = serviceType,
            Status = status
        };
        await _serviceAppointmentService.BookAppointmentAsync(appointment);
    }

    [Function(nameof(UpdateAppointmentStatus))]
    public async Task UpdateAppointmentStatus(
        [McpToolTrigger("UpdateAppointmentStatus", "Update the status of a service appointment. / อัปเดตสถานะของการนัดหมายบริการ")] ToolInvocationContext context,
        [McpToolProperty("appointmentId", "string", "Appointment ID (send as Guid string) / รหัสการนัดหมาย (ต้องส่งเป็น Guid string)")] string appointmentId,
        [McpToolProperty("status", "string", "New status / สถานะใหม่")] string status,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(appointmentId, out var guidAppointmentId))
            return;
        await _serviceAppointmentService.UpdateAppointmentStatusAsync(guidAppointmentId, status);
    }
}
