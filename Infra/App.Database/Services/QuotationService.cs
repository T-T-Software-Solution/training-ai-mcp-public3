using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App.AppCore.Interfaces;
using App.AppCore.Models;

namespace App.Database.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly AppContext _context;

        public QuotationService(AppContext context)
        {
            _context = context;
        }

        public async Task<Quotation?> GetQuotationByIdAsync(Guid id)
        {
            return await _context.Quotations
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Quotation>> GetQuotationsByCustomerIdAsync(Guid customerId)
        {
            return await _context.Quotations
                .Include(q => q.Customer)
                .Where(q => q.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task CreateQuotationAsync(Quotation quotation)
        {
            if (quotation.Id == Guid.Empty)
                quotation.Id = Guid.NewGuid();
            _context.Quotations.Add(quotation);
            await _context.SaveChangesAsync();
        }
    }
}
