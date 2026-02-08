using LostAndFound.Data;
using LostAndFound.Models;
using LostAndFound.Models.DTOs;
using LostAndFound.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Services.Implementations;

public class ItemService : IItemService
{
    private readonly ApplicationDbContext _context;
    private readonly IImageService _imageService;
    private readonly INotificationService _notificationService;
    private object userId;

    public ItemService(ApplicationDbContext context, IImageService imageService, INotificationService notificationService)
    {
        _context = context;
        _imageService = imageService;
        _notificationService = notificationService;
    }

    public object TotalCount { get; private set; }

    public async Task<Item?>GetItemByIdAsync(int id)
    {
        return await _context.Items
            .Include(i => i.ReportedByUser)
            .Include(i => i.FoundByUser)
            .Include(i => i.Comments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
    async Task<(IEnumerable<Item> Items, int TotalCount)>GetItemsAsync(ItemFilterDto filter)
    {
        var query = _context.Items
        .Include(i => i.ReportedByUser)
        .Include(i => i.FoundByUser)
        .AsQueryable();

        if (filter.Status.HasValue)
            query = query.Where(i => i.Status == filter.Status.Value);
        if(filter.Category.HasValue)
            query=query.Where(i=>i.Category == filter.Category.Value);
        if(!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchLower=filter.SearchTerm.ToLower();
            query=query.Where(i=>
            i.Title.ToLower().Contains(searchLower)||
            i.Description.ToLower().Contains(searchLower)||
            (i.Brand!=null&&i.Brand.ToLower().Contains(searchLower)));

        }
        if(!string.IsNullOrWhiteSpace(filter.Location))
        {
            var locationLower = filter.Location.ToLower();
            query = query.Where(i =>i.Location != null && i.Location.ToLower().Contains(locationLower));

        }
        if (filter.DateForm.HasValue)
            query = query.Where(i => i.DateReported >= filter.DateForm.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(i => i.DateReported <= filter.DateTo.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.DateReported)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        return (items, totalCount);

    }

    public async Task <Item>CreateItemAsync(CreateItemDto dto, string userId)
    {
        var item = new Item
        {
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            status = dto.Status,
            Location=dto.Location,
            Color=dto.Color,
            Brand=dto.Brand,
            ReportedByUserId=userId
        };
        if (dto.Image!=null)
        {
            item.ImageUrl = await _imageService.SaveImageAsync(dto.Image);
        }
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }
    public async  Task<Item?> UpdateItemAsync(int id, UpdateItemDto dto, string userId)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null || item.ReportedByUserId != userId)
            return null;
        if (!string.IsNullOrWhiteSpace(dto.Title))
            item.Title = dto.Title;
        if (!string.IsNullOrWhiteSpace(dto.Description))
            item.Description = dto.Description;
        if (dto.Category.HasValue)
            item.Category = dto.Category.Value;

        item.Location = dto.Location;
        item.Color = dto.Color;
        item.Brand = dto.Brand;

        if(dto.Image !=null)
        {
            if (!string.IsNullOrEmpty(item.ImageUrl))
                await _imageService.DeleteImageAsync(item.ImageUrl);

            item.ImageUrl = await _imageService.SaveImageAsync(dto.Image);

        }
        await _context.SaveChangesAsync();
        return item;
    }
    public async Task<bool> DeleteItemAsync(int id, string UserId, bool isAdmin)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null || (!isAdmin && item.ReportedByUserId != userId))
            return false;
        if (!string.IsNullOrEmpty(item.ImageUrl))
            await _imageService.DeleteImageAsync(item.ImageUrl);
        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<Item?>UpdateItemStausAsync(int id, UpdateItemStatusDto dto, string userId, bool isAdmin)
    {
        if (!isAdmin)
            return null;
        var item = await _context.Items
            .Include(i => i.ReportedByUser)
            .FirstOrDefaultAsync(i => i.Id ==id);
        if (item == null)
            return null;
        var oldStatus = item.Status;
        item.Status = dto.Status;
    if(dto.Status==ItemStatus.Found&& item.DateFound==null)
        {
            item.DateFound = DateTime.UtcNow;
            item.FoundByUserId = userId;
        }
    if(dto.Status==ItemStatus.Returned && item.DateReturned==null)
        {
            item.DateReturned = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();
        if (oldStatus!=dto.Status)
        {
            await _notificationService.NotifyItemStatusChangeAsync(item, oldStatus, dto.Status);
        }
        return item;
    }
   public async Task<ItemComment>AddCommentAsync(int itemId,CreateCommentDto dto, string userId)
    {
        var comment = new ItemComment
        {
            ItemId = itemId,
            UserId = userId,
            Content = dto.Content
        };
        _context.ItemComments.Add(comment);
        await _context.SaveChangesAsync();

        var item = await _context.Items.FindAsync(itemId);
        if(item !=null && item.ReportedByUserId !=userId)
        {
            await _notificationService.NotifyNewCommentAsync(item, userId);
        }
        return comment;
    }
    public async Task<IEnumerable<ItemComment>>GetItemCommentsAsync(int itemId)
    {
        return await _context.ItemComments
            .Include(c => c.User)
            .Where(c => c.ItemId == itemId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    Task<(IEnumerable<Item> Items, int TotalCount)> IItemService.GetItemsAsync(ItemFilterDto filter)
    {
        return GetItemsAsync(filter);
    }

    public Task<Item?> UpdateItemStatusAsync(int id, UpdateItemStatusDto dto, string UserId, bool isAdmin)
    {
        throw new NotImplementedException();
    }

    public Task<(object items, double totalCount)> GetItemIdAsync(ItemFilterDto filter)
    {
        throw new NotImplementedException();
    }

    public Task GetItemIdAsync(object id)
    {
        throw new NotImplementedException();
    }
}