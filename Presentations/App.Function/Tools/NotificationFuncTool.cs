using App.AppCore.Interfaces;
using App.AppCore.Models;
using App.AppCore.ToolInformations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Function.Tools;

public class NotificationFuncTool
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationFuncTool> _logger;

    public NotificationFuncTool(INotificationService notificationService, ILogger<NotificationFuncTool> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    [Function(nameof(SendNotification))]
    public async Task SendNotification(
        [McpToolTrigger("SendNotification", "Send a notification to a customer. / ส่งการแจ้งเตือนไปยังลูกค้า")] ToolInvocationContext context,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("content", "string", "Notification content / เนื้อหาการแจ้งเตือน")] string content,
        [McpToolProperty("type", "string", "Notification type (SMS, Email) / ประเภทการแจ้งเตือน")] string? type = null,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(customerId, out var guidCustomerId))
            return;
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            CustomerId = guidCustomerId,
            Content = content,
            Type = type,
            SentDate = DateTime.UtcNow
        };
        await _notificationService.SendNotificationAsync(notification);
        _logger.LogInformation($"Notification sent to customer {customerId}");
    }

    [Function(nameof(GetNotificationsByCustomerId))]
    public async Task<IEnumerable<Notification>> GetNotificationsByCustomerId(
        [McpToolTrigger("GetNotificationsByCustomerId", "Get notifications for a customer. / ดึงการแจ้งเตือนของลูกค้า")] ToolInvocationContext context,
        [McpToolProperty("customerId", "string", "Customer ID (send as Guid string) / รหัสลูกค้า (ต้องส่งเป็น Guid string)")] string customerId,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        if (!Guid.TryParse(customerId, out var guidCustomerId))
            return Enumerable.Empty<Notification>();
        return await _notificationService.GetNotificationsByCustomerIdAsync(guidCustomerId);
    }
}
