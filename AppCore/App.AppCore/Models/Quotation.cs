using System;

namespace App.AppCore.Models
{
    public class Quotation
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string? VehicleModel { get; set; }
        public string? VehicleGrade { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
