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
    public class AuthUseCase(IUserRepository userRepository) : IAuthUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User?> LoginAsync(string email, string password)
        {
            return await _userRepository.LoginUserAsync(email, password);
        }

        public async Task RegisterAsync(string email, string password, string confirmPassword)
        {
            await _userRepository.RegisterUserAsync(email, password, confirmPassword);
        }
    }
}
