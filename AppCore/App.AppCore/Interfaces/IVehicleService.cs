using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.AppCore.Models;

namespace App.AppCore.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicle?> GetVehicleByIdAsync(Guid id);
        Task<IEnumerable<Vehicle>> GetVehiclesByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string? model, string? color);
        Task AddVehicleAsync(Vehicle vehicle);
        Task UpdateVehicleAsync(Vehicle vehicle);
    }
}
