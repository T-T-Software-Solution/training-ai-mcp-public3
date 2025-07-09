using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.AppCore.Models;
using App.AppCore.Models.Dtos;

namespace App.AppCore.Interfaces
{
    public interface IServiceAppointmentService
    {
        Task<ServiceAppointment?> GetAppointmentByIdAsync(Guid id);
        Task<IEnumerable<ServiceAppointment>> GetAppointmentsByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<ServiceAppointment>> GetAvailableSlotsAsync(DateTime date, string serviceType);
        Task BookAppointmentAsync(ServiceAppointment appointment);
        Task UpdateAppointmentStatusAsync(Guid appointmentId, string status);
    }
}
