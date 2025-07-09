using System;

namespace App.AppCore.Models.Dtos
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public string? LicensePlate { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public Guid CustomerId { get; set; }
    }
}