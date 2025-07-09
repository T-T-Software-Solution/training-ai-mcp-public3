using App.AppCore.Models;
using App.AppCore.Interfaces;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace App.Mcp.Tools;

[McpServerToolType]
public class NotificationTools
{
    private readonly INotificationService _notificationService;

    public NotificationTools(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [McpServerTool, Description("Send a notification to a customer. / ส่งการแจ้งเตือนไปยังลูกค้า")]
    [Authorize]
    public async Task SendNotification(
        [Description("Customer ID to notify (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId,
        [Description("Notification type (e.g., SMS, Email) / ประเภทการแจ้งเตือน (เช่น SMS, Email)")] string type,
        [Description("Notification content/message / เนื้อหาหรือข้อความการแจ้งเตือน")] string content)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Type = type,
            Content = content,
            SentDate = DateTime.UtcNow
        };
        await _notificationService.SendNotificationAsync(notification);
    }

    [McpServerTool, Description("Get all notifications for a customer. / ดึงการแจ้งเตือนทั้งหมดของลูกค้า")]
    [Authorize]
    public async Task<IEnumerable<Notification>> GetNotificationsByCustomer(
        [Description("Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)")] Guid customerId)
    {
        return await _notificationService.GetNotificationsByCustomerIdAsync(customerId);
    }
}