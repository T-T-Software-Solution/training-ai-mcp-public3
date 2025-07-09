using System;
using System.Threading.Tasks;
using App.AppCore.Models;

namespace App.AppCore.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsByCustomerIdAsync(Guid customerId);
    }
}
