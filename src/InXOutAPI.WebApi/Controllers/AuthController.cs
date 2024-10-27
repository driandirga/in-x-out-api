using InXOutAPI.Application.DTOs;
using InXOutAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InXOutAPI.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthUseCase authUseCase) : ControllerBase
    {
        private readonly IAuthUseCase _authUseCase = authUseCase;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _authUseCase.LoginAsync(request.Email!, request.Password!);

                return Ok(new { message = "Login successful", user });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _authUseCase.RegisterAsync(request.Email!, request.Password!, request.ConfirmPassword!);
                return Ok(new { message = "Registration successful" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
