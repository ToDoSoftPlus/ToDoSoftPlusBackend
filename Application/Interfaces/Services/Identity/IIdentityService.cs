using Application.DTOs.Identity;

namespace Application.Interfaces.Services.Identity
{
    public interface IIdentityService
    {
        Task<AuthResponse> RegisterAsync(RegisterDto registerDto, CancellationToken token);
        Task<AuthResponse> LoginAsync(LoginDto loginDto, CancellationToken token);
        Task LogoutAsync(int userId, CancellationToken token);
    }
}
