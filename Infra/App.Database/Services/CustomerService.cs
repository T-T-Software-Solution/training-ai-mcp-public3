using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App.AppCore.Interfaces;
using App.AppCore.Models;

namespace App.Database.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppContext _context;

        public CustomerService(AppContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers
                .Include(c => c.Vehicles)
                .Include(c => c.ServiceAppointments)
                .Include(c => c.Quotations)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetCustomerByPhoneAsync(string phoneNumber)
        {
            return await _context.Customers
                .Include(c => c.Vehicles)
                .Include(c => c.ServiceAppointments)
                .Include(c => c.Quotations)
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.Vehicles)
                .Include(c => c.ServiceAppointments)
                .Include(c => c.Quotations)
                .ToListAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            if (customer.Id == Guid.Empty)
                customer.Id = Guid.NewGuid();
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
