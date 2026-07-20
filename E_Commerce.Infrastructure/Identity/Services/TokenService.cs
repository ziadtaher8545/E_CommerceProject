using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    internal class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtsettings;
        public TokenService(IOptions<JwtSettings> jwtoptions)
        {
            _jwtsettings = jwtoptions.Value;

        }
        public string CreateToken(string userId, string email, string userName, IEnumerable<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,userId),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Name,userName)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            if (string.IsNullOrWhiteSpace(_jwtsettings.SecretKey))
                throw new InvalidOperationException("JWT SecretKey is missing");

            if (_jwtsettings.SecretKey.Length < 32)
                throw new InvalidOperationException("JWT SecretKey is too short");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtsettings.Issuer, audience: _jwtsettings.Audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(_jwtsettings.ExpirationMinutes), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
    public class JwtSettings
    {
        public string SecretKey { get; init; } = default!;
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public int ExpirationMinutes { get; init; } = 60;
    }
}
