using BackendGrenishop.DTOs.Request;
using BackendGrenishop.DTOs.Response;

namespace BackendGrenishop.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<UserProfileDto?> GetUserProfileAsync(string userId);
}
