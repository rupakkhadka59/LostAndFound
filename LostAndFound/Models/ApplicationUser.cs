using Microsoft.AspNetCore.Identity;

namespace LostAndFound.Models;

public class ApplicationUser :IdentityUser
{
    public string? FullName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Item> ReportedItems { get; set; } = new List<Item>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

}