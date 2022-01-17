using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.BLL.Services.Interfaces;

namespace PrivateLibrary.BLL.Services.Realizations
{
    public class TokenService : ITokenService
    {
        //private const double ExpiryDurationMinutes = 30;

        private readonly ILogger<TokenService> _logger;
        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }
        public string? BuildToken(string? key, string? issuer, double expiryDurationMinutes, LoginDto? loginDto, IEnumerable<Claim>? claims)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(issuer);
            ArgumentNullException.ThrowIfNull(claims);
            ValidateLoginDto(loginDto);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddMinutes(expiryDurationMinutes), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public bool IsTokenValid(string? key, string? issuer, string? token)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(issuer);
            ArgumentNullException.ThrowIfNull(token);

            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = mySecurityKey,
                    }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void ValidateLoginDto(LoginDto? loginDto)
        {
            ArgumentNullException.ThrowIfNull(loginDto);
            Type dtoType = loginDto.GetType();
            var props = dtoType.GetProperties();
            foreach (var prop in props)
            {
                if (prop.GetValue(loginDto) == null)
                    throw new ArgumentException(nameof(loginDto));
            }
        }
    }

}
