using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FinanceTracker.Infrastructure.Security;

public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public TokenService(JwtSettings settings)
    {
        _settings = settings;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_settings.ExpiresInHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
