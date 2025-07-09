using System;
using System.Linq;
using App.AppCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Database
{
    public static class DbInitializer
    {
        public static void Seed(AppContext context)
        {
            if (context.Customers.Any()) return; // Already seeded

            // Customers
            var customer1 = new Customer { Id = Guid.NewGuid(), Name = "Somchai", PhoneNumber = "0812345678", Email = "somchai@email.com" };
            var customer2 = new Customer { Id = Guid.NewGuid(), Name = "Somsri", PhoneNumber = "0898765432", Email = "somsri@email.com" };
            context.Customers.AddRange(customer1, customer2);

            // Vehicles
            var vehicle1 = new Vehicle { Id = Guid.NewGuid(), LicensePlate = "กข1234", Model = "Civic", Color = "Red", Customer = customer1 };
            var vehicle2 = new Vehicle { Id = Guid.NewGuid(), LicensePlate = "ขค5678", Model = "Accord", Color = "Black", Customer = customer2 };
            context.Vehicles.AddRange(vehicle1, vehicle2);

            // Technicians
            var tech1 = new Technician { Id = Guid.NewGuid(), Name = "Technician A", Skills = "Engine" };
            var tech2 = new Technician { Id = Guid.NewGuid(), Name = "Technician B", Skills = "Electrical" };
            context.Technicians.AddRange(tech1, tech2);

            // StockItems
            var stock1 = new StockItem { Id = Guid.NewGuid(), Model = "Civic", Color = "Red", Quantity = 3 };
            var stock2 = new StockItem { Id = Guid.NewGuid(), Model = "Accord", Color = "Black", Quantity = 2 };
            context.StockItems.AddRange(stock1, stock2);

            // ServiceAppointments
            var appointment1 = new ServiceAppointment
            {
                Id = Guid.NewGuid(),
                AppointmentDate = DateTime.Today.AddDays(1),
                ServiceType = "Maintenance",
                Status = "Booked",
                Customer = customer1,
                Vehicle = vehicle1,
                Technician = tech1
            };
            context.ServiceAppointments.Add(appointment1);

            // Quotations
            var quotation1 = new Quotation
            {
                Id = Guid.NewGuid(),
                Customer = customer2,
                VehicleModel = "Accord",
                VehicleGrade = "Top",
                Price = 1200000,
                Discount = 60000,
                Status = "Draft",
                CreatedDate = DateTime.Now
            };
            context.Quotations.Add(quotation1);

            // Notifications
            var notification1 = new Notification
            {
                Id = Guid.NewGuid(),
                Type = "SMS",
                Content = "Your appointment is booked!",
                SentDate = DateTime.Now,
                Customer = customer1,
                ServiceAppointment = appointment1
            };
            context.Notifications.Add(notification1);

            context.SaveChanges();
        }

        // Call this in Program.cs after app.MapControllers()
        public static void EnsureSeedData(this IHost app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<App.Database.AppContext>();
            context.Database.Migrate(); // Ensure DB is up to date
            Seed(context);
        }
    }
}
