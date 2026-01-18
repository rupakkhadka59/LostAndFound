using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace LostAndFound.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;
        public EventBookmark IsRead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public  int? ItemId{ get; set; }
        public virtual  Item? Item{ get; set; }
        public string UserId { get; internal set; }
    }
}