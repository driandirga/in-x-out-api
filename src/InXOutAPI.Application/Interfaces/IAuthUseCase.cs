using InXOutAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InXOutAPI.Application.Interfaces
{
    public interface IAuthUseCase
    {
        Task<User?> LoginAsync(string email, string password);
        Task RegisterAsync(string email, string password, string confirmPassword);
    }
}
