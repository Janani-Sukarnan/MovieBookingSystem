
using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.DTOs.Auth;
using MovieBookingSystem.Services.Interfaces;

namespace MovieBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _userService.LoginAsync(dto);

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message,
                token = result.Token
            });
        }
    }
}
