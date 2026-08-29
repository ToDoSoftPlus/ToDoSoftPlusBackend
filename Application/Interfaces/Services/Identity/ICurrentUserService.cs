using Application.DTOs.User;

namespace Application.Interfaces.Services.Identity
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        Task<UserDto> GetCurrentUserInfoAsync();
    }
}
