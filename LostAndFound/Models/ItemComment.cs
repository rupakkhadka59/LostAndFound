using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Models
{
    public class ItemComment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Foreign Keys
        public int ItemId{ get; set; }
        public virtual Item Item { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}