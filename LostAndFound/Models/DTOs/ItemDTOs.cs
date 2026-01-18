using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace LostAndFound.Models.DTOs;

public class CreateItemDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    public ItemCategory Category { get; set; }

    public ItemStatus Status { get; set; } = ItemStatus.Lost;

    [MaxLength(500)]
    public string Location { get; set; }
    [MaxLength(100)]
    public string? Color { get; set; }


    [MaxLength(100)]
    public string Brand { get; set; }
    public IFormFile? Image { get; set; }
}
public class UpdateItemDto
{
    [MaxLength(200)]
    public string? Title{ get; set; }

    public string? Description{ get; set; }
    public ItemCategory? Category { get; set; }

    [MaxLength(100)]
    public string Location{ get; set; }

    [MaxLength(100)]
    public string? Color{ get; set; }
    [MaxLength(100)]
    public string? Brand{ get; set; }
    public IFormFile Image{ get; set; }
}
public  class UpdateItemStatusDto
{
    [Required]
    public  ItemStatus Status { get; set; }
    public string? Notes{ get; set; }
}
public class ItemFilterDto
{
    public ItemStatus? Status { get; set; }
    public ItemCategory? Category { get; set; }
    public string? SearchTerm { get; set; }
    public string? Location { get; set; }
    public DateTime? DateForm { get; set; }
    public DateTime? DateTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class CreateCommentDto
{
    [Required]
    public string Content { get; set; } = string.Empty;
}