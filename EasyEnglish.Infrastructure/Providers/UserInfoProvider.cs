using EasyEnglish.Application.Contracts.Providers;

namespace EasyEnglish.Infrastructure.Providers;

/// <summary>
/// Провайдер информации о пользователе
/// </summary>
public class UserInfoProvider : IUserInfoProvider
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Почта
    /// </summary>
    public string Email { get; set; } = string.Empty;
}