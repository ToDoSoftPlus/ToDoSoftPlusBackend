using Application.DTOs.User;

namespace Application.DTOs.Identity
{
    public class AuthResponse
    {
        public UserDto UserInfo { get; set; } = null!;

        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
