using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.AppCore.Interfaces;
using App.AppCore.Models;

namespace App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] Notification notification)
        {
            await _notificationService.SendNotificationAsync(notification);
            return Ok();
        }
    }
}
