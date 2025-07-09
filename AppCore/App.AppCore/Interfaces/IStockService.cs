using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.AppCore.Models;

namespace App.AppCore.Interfaces
{
    public interface IStockService
    {
        Task<IEnumerable<StockItem>> GetStockItemsAsync(string? model, string? color);
        Task<StockItem?> GetStockItemByIdAsync(Guid id);
    }
}
