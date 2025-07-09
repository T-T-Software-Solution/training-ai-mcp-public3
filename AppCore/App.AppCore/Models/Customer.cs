using System;
using System.Collections.Generic;

namespace App.AppCore.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new();
        public List<ServiceAppointment> ServiceAppointments { get; set; } = new();
        public List<Quotation> Quotations { get; set; } = new();
    }
}
