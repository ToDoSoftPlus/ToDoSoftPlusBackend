namespace Application.Models.Identity
{
    public sealed class JwtToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
