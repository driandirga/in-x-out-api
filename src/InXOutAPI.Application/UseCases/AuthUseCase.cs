using InXOutAPI.Application.Interfaces;
using InXOutAPI.Domain.Entities;
using InXOutAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InXOutAPI.Application.UseCases
{
    public class AuthUseCase(IUserRepository userRepository, IOTPRepository oTPRepository) : IAuthUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IOTPRepository _oTPRepository = oTPRepository;

        public async Task<User?> LoginAsync(string email, string password)
        {
            return await _userRepository.LoginUserAsync(email, password);
        }

        public async Task RegisterAsync(string email, string password, string confirmPassword)
        {
            await _userRepository.RegisterUserAsync(email, password, confirmPassword);
        }

        public async Task CreateNewPasswordAsync(string email, string password, string confirmPassword)
        {
            await _userRepository.CreateNewPasswordAsync(email, password, confirmPassword);
        }

        public async Task<bool> SendOtpEmailAsync(string email)
        {
            return await _oTPRepository.SendOtpEmail(email);
        }

        public async Task<bool> ValidateOtpTokenAsync(string email, string otp)
        {
            return await _oTPRepository.ValidateOTPToken(email, otp);
        }
    }
}
