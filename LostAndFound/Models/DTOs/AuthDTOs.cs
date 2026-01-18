using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Models.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

}

public class RegisterDto
{
    [Required]
    public string FullName{ get; set; }=string.Empty;

    [Required]
    [EmailAddress]

    public string EmailAddress { get; set; } = string.Empty;


    [Required]
    public string Password { get; set; } = string.Empty;
    public string Email { get; internal set; }
}
    public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public IList<string> Role { get; set; }=new List<string>();
    public IList<string> Roles { get; internal set; }
}