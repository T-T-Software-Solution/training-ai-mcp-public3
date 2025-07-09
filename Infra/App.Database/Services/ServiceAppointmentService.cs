using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App.AppCore.Interfaces;
using App.AppCore.Models;
using App.AppCore.Models.Dtos; // Add this using
using System.Linq;

namespace App.Database.Services
{
    public class ServiceAppointmentService : IServiceAppointmentService
    {
        private readonly AppContext _context;

        public ServiceAppointmentService(AppContext context)
        {
            _context = context;
        }

        public async Task<ServiceAppointment?> GetAppointmentByIdAsync(Guid id)
        {
            return await _context.ServiceAppointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .Include(a => a.Technician)
                .Include(a => a.Notifications)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ServiceAppointmentDto?> GetAppointmentDtoByIdAsync(Guid id)
        {
            var appointment = await GetAppointmentByIdAsync(id);
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

        public async Task<IEnumerable<ServiceAppointment>> GetAppointmentsByCustomerIdAsync(Guid customerId)
        {
            return await _context.ServiceAppointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .Include(a => a.Technician)
                .Include(a => a.Notifications)
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceAppointmentDto>> GetAppointmentDtosByCustomerIdAsync(Guid customerId)
        {
            var appointments = await GetAppointmentsByCustomerIdAsync(customerId);
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

        public async Task<IEnumerable<ServiceAppointment>> GetAvailableSlotsAsync(DateTime date, string serviceType)
        {
            var bookedSlots = await _context.ServiceAppointments
                .Where(a => a.AppointmentDate.Date == date.Date && a.ServiceType == serviceType)
                .Select(a => a.AppointmentDate)
                .ToListAsync();

            return await _context.ServiceAppointments
                .Where(a => a.AppointmentDate.Date == date.Date && a.ServiceType == serviceType && !bookedSlots.Contains(a.AppointmentDate))
                .ToListAsync();
        }

        public async Task BookAppointmentAsync(ServiceAppointment appointment)
        {
            if (appointment.Id == Guid.Empty)
                appointment.Id = Guid.NewGuid();

            // If TechnicianId is null, pick the first available technician (or any logic you prefer)
            if (appointment.TechnicianId == null)
            {
                var technician = await _context.Technicians.FirstOrDefaultAsync();
                if (technician != null)
                    appointment.TechnicianId = technician.Id;
            }

            _context.ServiceAppointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentStatusAsync(Guid appointmentId, string status)
        {
            var appointment = await _context.ServiceAppointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
