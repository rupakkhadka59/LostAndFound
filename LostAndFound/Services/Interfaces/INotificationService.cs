using LostAndFound.Models;
using LostAndFound.Models.DTOs;

namespace LostAndFound.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyItemStatusChangeAsync(Item item, ItemStatus oldStatus, ItemStatus newStatus);
        Task NotifyNewCommentAsync(Item item, string commenterId);
        Task<IEnumerable<NotificationDto>> GetNotificationsAsync(string userId);
        Task<bool> MarkAsReadAsync(int notificationId, string userId);
    }
}
