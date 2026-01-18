using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Models
{
    public class Item
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public ItemCategory Category { get; set; }

        public ItemStatus status { get; set; } = ItemStatus.Lost;

        [MaxLength(200)]
        public string? Location { get; set; }

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DateFound { get; set; }
        public DateTime? DateReturned { get; set; }

        public string ImageUrl { get; set; }

        [MaxLength(200)]
        public string Color { get; set; }

        [MaxLength(200)]
        public string Brand { get; set; }
        //Foreign Keys

        public string ReportedByUserId { get; set; } = string.Empty;
        public virtual ApplicationUser ReportedByUser { get; set; } = null;

        public string? FoundByUserId{ get; set; }
        public virtual  ApplicationUser? FoundByUser { get; set; }
        public virtual ICollection<ItemComment> Comments { get; set; } = new List<ItemComment>();
    }
}
