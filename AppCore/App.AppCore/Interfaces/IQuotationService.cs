using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.AppCore.Models;

namespace App.AppCore.Interfaces
{
    public interface IQuotationService
    {
        Task<Quotation?> GetQuotationByIdAsync(Guid id);
        Task<IEnumerable<Quotation>> GetQuotationsByCustomerIdAsync(Guid customerId);
        Task CreateQuotationAsync(Quotation quotation);
    }
}
