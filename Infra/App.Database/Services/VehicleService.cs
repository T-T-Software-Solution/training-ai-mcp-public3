using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App.AppCore.Interfaces;
using App.AppCore.Models;
using System.Linq;

namespace App.Database.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly AppContext _context;

        public VehicleService(AppContext context)
        {
            _context = context;
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await _context.Vehicles
                .Include(v => v.Customer)
                .Include(v => v.ServiceAppointments)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByCustomerIdAsync(Guid customerId)
        {
            return await _context.Vehicles
                .Include(v => v.Customer)
                .Include(v => v.ServiceAppointments)
                .Where(v => v.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string? model, string? color)
        {
            var query = _context.Vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(model))
                query = query.Where(v => v.Model == model);
            if (!string.IsNullOrEmpty(color))
                query = query.Where(v => v.Color == color);
            return await query.ToListAsync();
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            if (vehicle.Id == Guid.Empty)
                vehicle.Id = Guid.NewGuid();
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            var existing = await _context.Vehicles.FindAsync(vehicle.Id);
            if (existing != null)
            {
                existing.LicensePlate = vehicle.LicensePlate;
                existing.Model = vehicle.Model;
                existing.Color = vehicle.Color;
                existing.CustomerId = vehicle.CustomerId;
                await _context.SaveChangesAsync();
            }
        }
    }
}
