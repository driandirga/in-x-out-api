using InXOutAPI.Application.Helpers;
using InXOutAPI.Domain.Entities;
using InXOutAPI.Domain.Interfaces;
using InXOutAPI.Infrastructure.Persistence;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace InXOutAPI.Infrastructure.Repositories
{
    public class OTPRepository(ApplicationDbContext context, IUserRepository userRepository) : IOTPRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IUserRepository _userRepository = userRepository;
        private const int OTP_LENGTH = 5;
        private const int OTP_EXPIRATION = 1;
        private const string SECRET_KEY = "my_super_deep_salt_secret_key_12345678!";

        public string GenerateOTPToken(string otp)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("otp", otp)
                };

                var token = new JwtSecurityToken(
                    issuer: "InXOut",
                    audience: "InXOut",
                    claims: claims,
                    expires: DateTime.Now.ToUniversalTime().AddMinutes(OTP_EXPIRATION),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ValidateOTPToken(string identifier, string inputOtp)
        {
            try
            {
                var user = await _userRepository.GetUserByIdentifier(identifier);
                var otpEntry = _context.OTPs.FirstOrDefault(o => o.UserId == user.Id && o.Code != null && o.ExpirationTime > DateTime.Now.ToUniversalTime())
                               ?? throw new ArgumentException("OTP not found.");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "InXOut",
                    ValidAudience = "InXOut",
                    IssuerSigningKey = key
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(otpEntry.Code, validationParameters, out SecurityToken validatedToken);

                var otpClaim = principal.Claims.FirstOrDefault(c => c.Type == "otp")?.Value;
                if (otpClaim == null)
                    return false;

                return otpClaim == inputOtp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<OTP?> SaveOTPAsync(string identifier)
        {
            try
            {
                var otpCode = Generate.RandomNumber(OTP_LENGTH);
                var otpToken = GenerateOTPToken(otpCode);
                var user = await _userRepository.GetUserByIdentifier(identifier);
                var userId = user.Id;

                var existingOtp = _context.OTPs.FirstOrDefault(o => o.UserId == userId && o.Code != null && o.ExpirationTime > DateTime.Now.ToUniversalTime());
                if (existingOtp != null)
                {
                    existingOtp.Code = otpToken;
                    existingOtp.ExpirationTime = DateTime.Now.ToUniversalTime().AddMinutes(OTP_EXPIRATION);
                    await _context.SaveChangesAsync();
                    return existingOtp;
                }

                var otp = new OTP
                {
                    UserId = userId,
                    Code = otpToken,
                    ExpirationTime = DateTime.Now.ToUniversalTime().AddMinutes(OTP_EXPIRATION),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                };

                _context.OTPs.Add(otp);
                await _context.SaveChangesAsync();

                return otp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SendOtpEmail(string identifier)
        {

            try
            {
                var otp = await SaveOTPAsync(identifier);
                if (otp == null)
                    return false;

                //var smtpClient = new SmtpClient("smtp.gmail.com")
                //{
                //    Port = 587,
                //    Credentials = new NetworkCredential("protentik@gmail.com", "rgcyqjoelcgkomvx"),
                //    EnableSsl = true,
                //};

                //var mailMessage = new MailMessage
                //{
                //    From = new MailAddress("protentik@gmail.com"),
                //    Subject = "Your OTP Code",
                //    Body = $"Your OTP code is: {otp?.Code}",
                //    IsBodyHtml = false,
                //};

                //mailMessage.To.Add(email);

                //await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
