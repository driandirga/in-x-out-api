using InXOutAPI.Application.Helpers;
using InXOutAPI.Domain.Entities;
using InXOutAPI.Domain.Interfaces;
using InXOutAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InXOutAPI.Infrastructure.Repositories
{
    public partial class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();

        public void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !EmailRegex().IsMatch(email))
                throw new ArgumentException("Invalid email format.");
        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long.");
        }

        public void ValidateConfirmPassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
                throw new ArgumentException("Passwords do not match.");
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            ValidateEmail(email);

            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdentifier(string identifier)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == identifier || u.Username == identifier || u.Phone == identifier)
                       ?? throw new ArgumentException("User not found.");

            return user;
        }

        public async Task RegisterUserAsync(string email, string password, string confirmPassword)
        {
            ValidateEmail(email);
            ValidatePassword(password);
            ValidateConfirmPassword(password, confirmPassword);

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
                throw new ArgumentException("Email is already registered.");

            string randomString;
            bool isUsernameUnique = false;

            do
            {
                randomString = Generate.RandomString(8);
                var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == randomString);
                if (existingUsername == null)
                {
                    isUsernameUnique = true;
                }
            }
            while (!isUsernameUnique);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = randomString,
                Email = email,
                Password = hashedPassword,
                RoleId = 1,
                CreatedAt = DateTime.Now.ToUniversalTime(),
                CreatedBy = "SYSTEM",
                UpdatedAt = DateTime.Now.ToUniversalTime(),
                UpdatedBy = "SYSTEM"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> LoginUserAsync(string email, string password)
        {
            ValidateEmail(email);
            ValidatePassword(password);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new ArgumentException("Login failed, email or password is incorrect.");

            return user;
        }

        public async Task CreateNewPasswordAsync(string email, string password, string confirmPassword)
        {
            ValidateEmail(email);
            ValidatePassword(password);
            ValidateConfirmPassword(password, confirmPassword);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email)
                       ?? throw new ArgumentException("Email is already registered.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            user!.Password = hashedPassword;

            await _context.SaveChangesAsync();
        }
    }
}
