using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    /// <summary>
    /// Get my notifications (requires authentication)
    /// </summary>
    [HttpGet]
    [Authorize]
    public IActionResult GetMyNotifications()
    {
        // TODO: Implement
        return Ok(new List<object>());
    }

    /// <summary>
    /// Mark notification as read (requires authentication)
    /// </summary>
    [HttpPut("{id}/read")]
    [Authorize]
    public IActionResult MarkNotificationRead(int id)
    {
        // TODO: Implement
        return Ok(new { message = "Notification marked as read" });
    }
}