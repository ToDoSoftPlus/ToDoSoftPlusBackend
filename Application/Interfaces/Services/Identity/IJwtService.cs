using Application.Models.Identity;
using Domain.Entities;

namespace Application.Interfaces.Services.Identity
{
    public interface IJwtService
    {
        public JwtToken GenerateAccessToken(ApplicationUser user, List<string> roles);
    }
}
