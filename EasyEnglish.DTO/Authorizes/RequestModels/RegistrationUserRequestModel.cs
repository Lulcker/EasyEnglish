namespace EasyEnglish.DTO.Authorizes.RequestModels;

/// <summary>
/// Входная модель регистрации пользователя
/// </summary>
public class RegistrationUserRequestModel
{
    /// <summary>
    /// Имя
    /// </summary>
    public required string FirstName { get; init; }
    
    /// <summary>
    /// Почта
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public required string Password { get; init; }
}