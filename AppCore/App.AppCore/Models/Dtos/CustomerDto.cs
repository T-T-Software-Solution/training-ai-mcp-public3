using System;

namespace App.AppCore.Models.Dtos
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}