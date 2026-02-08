using LostAndFound.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LostAndFound.Controllers;

[ApiController]
[Route("api/[Controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) 
            return Unauthorized();

        return Ok(await _notificationService.GetUserNotificationsAsync(userId));
    }

    private IActionResult Ok(object v)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id}/read")]
    public async Task <IActionResult>MarkAsRead(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        var result = await _notificationService.MarkAsReadAsync(id, userId);
        if (!result)
            return NotFound();

        return NoContent();
    }

}
