using System;
using System.Collections.Generic;

namespace App.AppCore.Models
{
    public class Technician
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Skills { get; set; }
        public List<ServiceAppointment> ServiceAppointments { get; set; } = new();
    }
}
