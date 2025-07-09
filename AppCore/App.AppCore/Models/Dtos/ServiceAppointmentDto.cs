using System;

namespace App.AppCore.Models.Dtos
{
    public class ServiceAppointmentDto
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? ServiceType { get; set; }
        public string? Status { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid? TechnicianId { get; set; }
    }
}