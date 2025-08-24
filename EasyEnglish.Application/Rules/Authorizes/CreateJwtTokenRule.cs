using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;
using EasyEnglish.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace EasyEnglish.Application.Rules.Authorizes;

/// <summary>
/// Правило создания JWT токена
/// </summary>
public class CreateJwtTokenRule(
    IAuthProvider authProvider,
    IAesCryptoService aesCryptoService
    )
{
    public string CreateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Sid, user.Id.ToString()),
            new (ClaimTypes.Email, aesCryptoService.Decrypt(user.Email)),
            new (ClaimTypes.Name, aesCryptoService.Decrypt(user.FirstName)),
        };

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: authProvider.Issuer,
            audience: authProvider.Audience,
            notBefore: now,
            claims: claims,
            expires: now.AddDays(60),
            signingCredentials: new SigningCredentials(authProvider.GetSecurityKey(), 
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}