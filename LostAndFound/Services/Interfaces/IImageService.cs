namespace LostAndFound.Services.Interfaces;

public interface IImageService
{
    public Task<string> SaveImageAsync(IFormFile image);
    public Task DeleteImageAsync(string imageUrl);
}
