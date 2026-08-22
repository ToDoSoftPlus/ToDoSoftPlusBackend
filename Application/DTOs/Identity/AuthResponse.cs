using Application.DTOs.User;

namespace Application.DTOs.Identity
{
    public class AuthResponse
    {
        public UserDto UserInfo { get; set; } = null!;

        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; set; }
    }
}
