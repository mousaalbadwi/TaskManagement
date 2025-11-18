using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.DTOs.Auth;
using TaskManagement.Api.Services.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequestDto request)
        {
            try
            {
                await _authService.SignupAsync(request);
                return Ok(new { Message = "User registered successfully." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
