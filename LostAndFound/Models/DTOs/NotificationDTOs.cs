using System.Diagnostics.Eventing.Reader;

namespace LostAndFound.Models.DTOs;

public class NotificationDto

{
    public int Id{ get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set;  }
    public int? ItemId{ get; set; }
}
