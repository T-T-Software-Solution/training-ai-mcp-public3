using System;
using System.Collections.Generic;

namespace App.AppCore.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string? LicensePlate { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public List<ServiceAppointment> ServiceAppointments { get; set; } = new();
    }
}
