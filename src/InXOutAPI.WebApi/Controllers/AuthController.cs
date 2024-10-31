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

        [HttpPost("create-new-password")]
        public async Task<IActionResult> CreateNewPassword([FromBody] CreateNewPasswordRequest request)
        {
            try
            {
                await _authUseCase.CreateNewPasswordAsync(request.Email!, request.Password!, request.ConfirmPassword!);

                return Ok(new { message = "Password updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("send-otp-email")]
        public async Task<IActionResult> SendOtpEmail([FromBody] SendOtpEmailRequest request)
        {
            try
            {
                bool isSent = await _authUseCase.SendOtpEmailAsync(request.Email!);

                return Ok(new { message = isSent ? "OTP sent successfully" : "Failed to send OTP" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("validate-otp-token")]
        public async Task<IActionResult> ValidateOtpToken([FromBody] ValidateOtpTokenRequest request)
        {
            try
            {
                bool isValid = await _authUseCase.ValidateOtpTokenAsync(request.Email!, request.OTP!);

                return Ok(new { message = isValid ? "OTP is valid" : "OTP is invalid" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
