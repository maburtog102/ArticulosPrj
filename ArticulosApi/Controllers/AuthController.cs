using ArticulosApi.Dtos;
using ArticulosApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticulosApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistroDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result) return BadRequest("Email ya registrado.");
            return Ok("Usuario registrado.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null) return Unauthorized("Credenciales inválidas.");
            return Ok(new { token });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = await _authService.GetUserAsync(email);
            return Ok(user);
        }
    }
}