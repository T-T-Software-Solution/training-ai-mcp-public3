using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App.AppCore.Interfaces;
using App.AppCore.Models;
using System.Linq;

namespace App.Database.Services
{
    public class StockService : IStockService
    {
        private readonly AppContext _context;

        public StockService(AppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockItem>> GetStockItemsAsync(string? model, string? color)
        {
            var query = _context.StockItems.AsQueryable();
            if (!string.IsNullOrEmpty(model))
                query = query.Where(s => s.Model == model);
            if (!string.IsNullOrEmpty(color))
                query = query.Where(s => s.Color == color);
            return await query.ToListAsync();
        }

        public async Task<StockItem?> GetStockItemByIdAsync(Guid id)
        {
            return await _context.StockItems.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
