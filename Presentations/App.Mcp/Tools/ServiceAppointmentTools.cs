using App.AppCore.Models;
using App.AppCore.Models.Dtos;
using App.AppCore.Interfaces;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace App.Mcp.Tools;

[McpServerToolType]
public class ServiceAppointmentTools
{
    private readonly IServiceAppointmentService _serviceAppointmentService;

    public ServiceAppointmentTools(IServiceAppointmentService serviceAppointmentService)
    {
        _serviceAppointmentService = serviceAppointmentService;
    }

    [McpServerTool, Description("Get service appointment details by appointment ID. / ดึงข้อมูลการนัดหมายด้วยรหัสการนัดหมาย (Appointment ID ต้องเป็น Guid)")]
    [Authorize]
    public async Task<ServiceAppointmentDto?> GetAppointmentById(
        [Description("Appointment ID (Guid) / รหัสการนัดหมาย (ต้องเป็น Guid)")] Guid id)
    {
        var appointment = await _serviceAppointmentService.GetAppointmentByIdAsync(id);
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

    [McpServerTool, Description("Get all service appointments for a customer. / ดึงการนัดหมายทั้งหมดของลูกค้า")]
    [Authorize]
    public async Task<IEnumerable<ServiceAppointmentDto>> GetAppointmentsByCustomer(
        [Description("Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId)
    {
        var appointments = await _serviceAppointmentService.GetAppointmentsByCustomerIdAsync(customerId);
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

    [McpServerTool, Description("Get available service appointment slots for a date and service type. / ดึงช่วงเวลานัดหมายที่ว่างสำหรับวันที่และประเภทบริการ")]
    [Authorize]
    public async Task<IEnumerable<ServiceAppointmentDto>> GetAvailableSlots(
        [Description("Date (yyyy-MM-dd) / วันที่ (yyyy-MM-dd)")] DateTime date,
        [Description("Service type / ประเภทบริการ")] string serviceType)
    {
        var slots = await _serviceAppointmentService.GetAvailableSlotsAsync(date, serviceType);
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

    [McpServerTool, Description("Book a new service appointment. / จองการนัดหมายบริการใหม่")]
    [Authorize]
    public async Task BookAppointment(
        [Description("Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId,
        [Description("Vehicle ID (Guid) / รหัสรถยนต์ (ต้องเป็น Guid)")] Guid vehicleId,
        [Description("Appointment date and time / วันและเวลานัดหมาย")] DateTime appointmentDate,
        [Description("Service type / ประเภทบริการ")] string serviceType,
        [Description("Status / สถานะ")] string? status
    )
    {
        var appointment = new ServiceAppointment
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            VehicleId = vehicleId,
            TechnicianId = null,
            AppointmentDate = appointmentDate,
            ServiceType = serviceType,
            Status = status
        };
        await _serviceAppointmentService.BookAppointmentAsync(appointment);
    }

    [McpServerTool, Description("Update the status of a service appointment. / อัปเดตสถานะของการนัดหมายบริการ")]
    [Authorize]
    public async Task UpdateAppointmentStatus(
        [Description("Appointment ID (Guid) / รหัสการนัดหมาย (ต้องเป็น Guid)")] Guid appointmentId,
        [Description("New status / สถานะใหม่")] string status)
    {
        await _serviceAppointmentService.UpdateAppointmentStatusAsync(appointmentId, status);
    }
}