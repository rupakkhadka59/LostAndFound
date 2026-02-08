using LostAndFound.Services.Interfaces;

namespace LostAndFound.Services.Implementations;

public class ImageService :IImageService
{
    private readonly string _uploadPath = "wwwroot/uploads/images";

    public ImageService()
    {
        if(!Directory.Exists(_uploadPath))
            Directory.CreateDirectory(_uploadPath); 
    }
    public async Task <string>SaveImageAsync(IFormFile image)
    {
        var  fileName=$"{Guid.NewGuid ()}{Path.GetExtension(image.FileName)}";
        var filePath = Path.Combine(_uploadPath, fileName);

        using(var stream=new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }
        return $"/uploads/image{fileName}";
    }
    public Task DeleteImageSync(string imageUrl)
    {
        var fileName=Path.GetFileName(imageUrl);
        var filePath = Path.Combine(_uploadPath, fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;

        
    }

    public Task DeleteImageAsync(string imageUrl)
    {
        throw new NotImplementedException();
    }
}
