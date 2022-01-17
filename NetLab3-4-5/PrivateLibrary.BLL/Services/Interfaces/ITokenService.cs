using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PrivateLibrary.BLL.Dtos.Auth;

namespace PrivateLibrary.BLL.Services.Interfaces
{
    public interface ITokenService
    {
        string? BuildToken(string? key, string? issuer, double expiryDurationMinutes, LoginDto? loginDto, IEnumerable<Claim>? claims);
        bool IsTokenValid(string? key, string? issuer, string? token);
    }
}
