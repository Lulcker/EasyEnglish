using System.Text;
using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Infrastructure.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EasyEnglish.Configurators;

internal static class AuthConfigurator
{
    internal static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
    {
        var key = builder.Configuration.GetValue<string>("Auth:Key") 
                  ?? throw new ArgumentNullException("Auth:Key");
        var issuer = builder.Configuration.GetValue<string>("Auth:Issuer") 
                     ?? throw new ArgumentNullException("Auth:Issuer");
        var audience = builder.Configuration.GetValue<string>("Auth:Audience") 
                       ?? throw new ArgumentNullException("Auth:Audience");
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuerSigningKey = true,
                };
            });

        builder.Services.AddSingleton<IAuthProvider>(_ => new AuthProvider
        {
            Key = key,
            Issuer = issuer,
            Audience = audience,
        });
        
        builder.Services.AddScoped<UserInfoProvider>();
        builder.Services.AddScoped<IUserInfoProvider>(provider => provider.GetRequiredService<UserInfoProvider>());
        
        return builder;
    }
}