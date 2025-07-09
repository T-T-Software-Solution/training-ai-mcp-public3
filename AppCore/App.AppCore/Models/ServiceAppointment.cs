using System;
using System.Collections.Generic;

namespace App.AppCore.Models
{
    public class ServiceAppointment
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? ServiceType { get; set; }
        public string? Status { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public Guid? TechnicianId { get; set; }
        public Technician? Technician { get; set; }
        public List<Notification> Notifications { get; set; } = new();
    }
}
