using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ManagementData.Api.Contracts.Auth;
using Microsoft.IdentityModel.Tokens;

namespace ManagementData.Api.Features.Auth;

public sealed class JwtTokenService(JwtSettings jwtSettings) : ITokenService
{
    public TokenResponse CreateToken(AuthTokenContext context)
    {
        var expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, context.User.UserId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, context.User.UserId)
        };

        if (!string.IsNullOrWhiteSpace(context.User.UserName))
        {
            claims.Add(new Claim(ClaimTypes.Name, context.User.UserName));
            claims.Add(new Claim("preferred_username", context.User.UserName));
        }

        if (!string.IsNullOrWhiteSpace(context.User.Email))
        {
            claims.Add(new Claim(ClaimTypes.Email, context.User.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, context.User.Email));
        }

        if (context.PracticeId.HasValue)
        {
            claims.Add(new Claim("practice_id", context.PracticeId.Value.ToString()));
        }

        foreach (var role in context.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new TokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresInSeconds = (int)TimeSpan.FromMinutes(jwtSettings.ExpiryMinutes).TotalSeconds,
            UserId = context.User.UserId,
            UserName = context.User.UserName,
            Email = context.User.Email,
            PracticeId = context.PracticeId,
            Roles = context.Roles
        };
    }
}