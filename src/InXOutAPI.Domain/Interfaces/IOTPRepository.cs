using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InXOutAPI.Domain.Interfaces
{
    public interface IOTPRepository
    {
        string GenerateOTPToken(string otp);
        Task<bool> ValidateOTPToken(string otp, string identifier);
        Task<bool> SendOtpEmail(string email);
    }
}
