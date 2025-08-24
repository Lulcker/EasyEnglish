using System.Security.Claims;
using EasyEnglish.Infrastructure.Providers;

namespace EasyEnglish.Middlewares;

internal class UserInfoMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var userInfoProvider = context.RequestServices.GetRequiredService<UserInfoProvider>();

        var userName = context.User.Identity?.Name;

        if (userName is null)
        {
            await next.Invoke(context);
            return;
        }
        
        userInfoProvider.Id = Guid.Parse(context.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Sid)!.Value);
        userInfoProvider.Email = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
        userInfoProvider.FirstName = userName;

        await next.Invoke(context);
    }
}

internal static class MiddlewareExtensions
{
    internal static void UseUserInfo(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<UserInfoMiddleware>();
    }
}