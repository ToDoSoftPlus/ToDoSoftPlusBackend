using Application.DTOs.Identity;
using Application.Interfaces.Services.Identity;
using Application.Interfaces.Services.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidationService _validationService;

        public AuthController(IIdentityService identityService, ICurrentUserService currentUserService, IValidationService validationService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
            _validationService = validationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken token)
        {
            await _validationService.ValidateAsync(registerDto, token);

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
