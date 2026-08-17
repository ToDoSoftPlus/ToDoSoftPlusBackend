using Application.Interfaces.Services.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int UserId
        {
            get
            {
                var userId = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException(
                        "User is not authenticated.");
                }

                if (!int.TryParse(userId, out var id))
                {
                    throw new UnauthorizedAccessException(
                        "Invalid user ID.");
                }

                return id;
            }
        }
    }
}
