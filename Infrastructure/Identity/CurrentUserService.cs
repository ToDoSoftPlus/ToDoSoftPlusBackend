using Application.DTOs.User;
using Application.Interfaces.Services.Identity;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
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

        public async Task<UserDto> GetCurrentUserInfoAsync()
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());

            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            };

            return _mapper.Map<UserDto>(user);
        }
    }
}
