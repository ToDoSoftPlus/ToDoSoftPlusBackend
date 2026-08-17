using Application.DTOs.Identity;
using Application.Interfaces.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken token)
        {
            var authResponse = await _identityService.RegisterAsync(registerDto, token);
            return Ok(authResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken token)
        {
            var authResponse = await _identityService.LoginAsync(loginDto, token);
            return Ok(authResponse);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken token)
        {
            var userId = _currentUserService.UserId;
            await _identityService.LogoutAsync(userId, token);
            return Ok();
        }
    }
}
