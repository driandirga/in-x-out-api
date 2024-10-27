using InXOutAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InXOutAPI.Domain.Interfaces
{
    public interface IUserRepository
    {
        void ValidateEmail(string email);
        void ValidatePassword(string confirmPassword);
        void ValidateConfirmPassword(string password, string confirmPassword);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<User> GetUserByIdentifier(string identifier);
        Task RegisterUserAsync(string email, string password, string confirmPassword);
        Task<User?> LoginUserAsync(string email, string password);
        Task CreateNewPasswordAsync(string email, string password, string confirmPassword);
    }
}
