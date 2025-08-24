using System.Text;
using EasyEnglish.Application.Contracts.Providers;
using Microsoft.IdentityModel.Tokens;

namespace EasyEnglish.Infrastructure.Providers;

/// <summary>
/// Провайдер авторизации
/// </summary>
public class AuthProvider : IAuthProvider
{
    public required string Key { get; init; }
    
    public required string Issuer { get; init; }
    
    public required string Audience { get; init; }
    
    public SymmetricSecurityKey GetSecurityKey() => 
        new(Encoding.ASCII.GetBytes(Key));
}