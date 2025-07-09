using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.AppCore.Models;

namespace App.AppCore.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer?> GetCustomerByPhoneAsync(string phoneNumber);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
    }
}
