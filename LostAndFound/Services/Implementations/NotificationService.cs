using LostAndFound.Models;
using LostAndFound.Services.Interfaces;

namespace LostAndFound.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;

    }
    public async Task NotifyItemStatusChangeAsync(Item item, ItemStatus oldStatus, ItemStatus newStatus)
    {
        var message = $"Your item '{item.Title}'status changed  from {oldStatus} to {newStatus}";
        var notification = new Notification
        {
            UserId = item.ReportedByUserId,
            ItemId = item.Id,
            Message = message
        };
        _context.Notification.Add(notification);
        await _context.SaveChangesAsync();
    }
    public async Task NotifyNewCommentAsync(Item item, string commenterId)
    {
        var commenter = await _context.Users.FindAsync(commenterId);
        var message = $"{commenter?.FullName ?? "Someone"}Commented on your Item '{item.Title}'";
        var notification = new Notification
        {
            UserId = item.ReportedByUserId,
            ItemId = item.Id,
            Message = message
        };
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

    }
    public async Task<IEnumerable<NotificationDto>> GetUserNotificationAsync(string userId
        {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDto
            {
                Id - n.Id,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                ItemId = n.ItemId
            })
            .ToListAsync();
    }
    public async Task<bool>MarkAsReadAsync(int notificationId, string UserId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);
        if (notification == null)
            return false;

        notification.IsRead = true;
        await _context.SaveChangesASsync();
        return true;s
    }
}