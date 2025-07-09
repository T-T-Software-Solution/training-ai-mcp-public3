using System;

namespace App.AppCore.Models
{
    public class StockItem
    {
        public Guid Id { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; }
    }
}
