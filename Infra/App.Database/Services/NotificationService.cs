using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.AppCore.Interfaces;
using App.AppCore.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Database.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppContext _context;

        public NotificationService(AppContext context)
        {
            _context = context;
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            if (notification.Id == Guid.Empty)
                notification.Id = Guid.NewGuid();
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByCustomerIdAsync(Guid customerId)
        {
            return await _context.Notifications
                .Where(n => n.CustomerId == customerId)
                .OrderByDescending(n => n.SentDate)
                .ToListAsync();
        }
    }
}
