using Application.DTOs.Identity;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces.Services.Identity;
using AutoMapper;
using Domain.Constant;
using Domain.Entities;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public IdentityService(IJwtService jwtService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuthResponse> LoginAsync(LoginDto loginDto, CancellationToken token)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                throw new BusinessException("Invalid email or password.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordValid)
            {
                throw new BusinessException("Invalid email or password.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtService.GenerateAccessToken(user, userRoles.ToList());

            return new AuthResponse
            {
                UserInfo = _mapper.Map<UserDto>(user),
                AccessToken = accessToken.Token,
                AccessTokenExpiresAt = accessToken.ExpiresAt
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto, CancellationToken token)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

            if (existingUser is not null)
            {
                throw new BusinessException("User with this email already exists.");
            }
            
            var user = _mapper.Map<RegisterDto, ApplicationUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new IdentityException(result.Errors);
            }

            var roles = new List<string>();

            if (_userManager.Users.Count() == 1)
            {
                roles.Add(UserRoles.Admin);
            }
            else
            {
                roles.Add(UserRoles.User);
            }

            var roleResult = await _userManager.AddToRolesAsync(user, roles);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new IdentityException(roleResult.Errors);
            }

            var accessToken = _jwtService.GenerateAccessToken(user, roles);

            return new AuthResponse
            {
                UserInfo = _mapper.Map<UserDto>(user),
                AccessToken = accessToken.Token,
                AccessTokenExpiresAt = accessToken.ExpiresAt
            };
        }

        public async Task LogoutAsync(int userId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
