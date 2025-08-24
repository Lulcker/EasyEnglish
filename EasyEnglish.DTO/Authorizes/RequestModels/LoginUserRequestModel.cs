namespace EasyEnglish.DTO.Authorizes.RequestModels;

/// <summary>
/// Входная модель для авторизации пользователя
/// </summary>
public class LoginUserRequestModel
{
    /// <summary>
    /// Почта
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public required string Password { get; init; }
}