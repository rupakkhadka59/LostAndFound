using LostAndFound.Models;
using LostAndFound.Models.DTOs;

namespace LostAndFound.Services.Interfaces;

public interface IItemService
{
    Task<Item?>GetItemIdAsync(int id);
    Task<(IEnumerable<Item> Items, int TotalCount)> GetItemsAsync(ItemFilterDto filter);
    Task<Item> CreateItemAsync(CreateItemDto dto, string userId);
    Task<Item?>UpdateItemAsync(int id,UpdateItemDto dto, string userId) ;
    Task<bool> DeleteItemAsync(int id, string UserId, bool isAdmin);
    Task<Item?>UpdateItemStatusAsync(int id, UpdateItemStatusDto dto, string UserId, bool isAdmin);
    Task<ItemComment> AddCommentAsync(int itemId, CreateCommentDto dto, string userId);
    Task<IEnumerable<ItemComment>> GetItemCommentsAsync(int itemId);
    Task<Item?> UpdateItemStausAsync(int id, UpdateItemStatusDto dto, string userId, bool isAdmin);
}
