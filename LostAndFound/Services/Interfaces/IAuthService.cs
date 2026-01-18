using LostAndFound.Models.DTOs;

namespace LostAndFound.Services.Interfaces;

public interface IAuthServices
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
}
