using System;

namespace App.AppCore.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string? Type { get; set; } // SMS, Email
        public string? Content { get; set; }
        public DateTime SentDate { get; set; }
        public Guid? ServiceAppointmentId { get; set; }
        public ServiceAppointment? ServiceAppointment { get; set; }
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
