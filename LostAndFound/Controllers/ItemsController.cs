using LostAndFound.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LostAndFound.Models.DTOs;

namespace LostAndFound.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService; 
    
      public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetItems([FromQuery]ItemFilterDto filter)
    {
        var (items, totalCount) = await _itemService.GetItemIdAsync(filter);
        return Ok(new

        {
            items,
            totalCount,
            pageNumber = filter.PageNumber,
            pageSize = filter.PageSize,
            totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        });
    }
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await _itemService.GetItemByIdAsync(id);
        if (item == null)
        
            return NotFound();

            return Ok(item);
        
    }
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromForm] CreateItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var item = await _itemService.CreateItemAsync(dto, userId);
            return CreatedAtAction(nameof(GetItem), new { id = item.id }, item);
        }
        [HttpPut("{id}")]
            public async Task<IActionResult>UpdateItem(int id, [FromForm]UpdateItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
        
    var item = await _itemService.UpdateItemAsync(id, dto, userId);
        if (item == null)
            return NotFound();

        return Ok(item);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult>DeleteItem(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        var isAdmin = User.IsInRole("Admin");
        var result = await _itemService.DeleteItemAsync(id, userId, isAdmin);

        if (!result)
            return NotFound();
        return NoContent();
    }
    [HttpPatch("{id}/status")]
    [Authorize(Roles ="Admin")]
    public async Task <IActionResult>UpdateItemStatus(int id, [FromBody]UpdateItemStatusDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        var item = await _itemService.UpdateItemStatusAsync(id, dto, userId, true);
        if (item == null)
            return NotFound();
        return Ok(item);
    }
    [HttpGet("{id}/comments")]
    public async Task <IActionResult>GetComments(int id)
    {
        var comments = await _itemService.GetItemCommentsAsync(id);
        return Ok(comments);
    }
    [HttpPost("{id}/comments")]
    public async Task <IActionResult>AddComment (int id, [FromBody]CreateCommentDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var comment = await _itemService.AddCommentAsync(id, dto, userId);
        return Ok(comment);
    }
}

