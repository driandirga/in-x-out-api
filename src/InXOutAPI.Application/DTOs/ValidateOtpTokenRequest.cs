using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InXOutAPI.Application.DTOs
{
    public class ValidateOtpTokenRequest
    {
        public string? Email { get; set; }
        public string? OTP { get; set; }
    }
}
