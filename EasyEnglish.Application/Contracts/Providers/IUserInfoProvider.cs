namespace EasyEnglish.Application.Contracts.Providers;

/// <summary>
/// Провайдер пользователя
/// </summary>
public interface IUserInfoProvider
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// Имя
    /// </summary>
    string FirstName { get; }
    
    /// <summary>
    /// Почта
    /// </summary>
    string Email { get; }
}