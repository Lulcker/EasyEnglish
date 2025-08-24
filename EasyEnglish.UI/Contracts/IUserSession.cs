using EasyEnglish.DTO.Authorizes.ResponseModels;

namespace EasyEnglish.UI.Contracts;

public interface IUserSession
{
    /// <summary>
    /// Токен доступа пользователя
    /// </summary>
    string Token { get; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    string FirstName { get; }
    
    /// <summary>
    /// Почта пользователя
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Начать сессию модератора
    /// </summary>
    /// <param name="responseModel">Входная модель</param>
    Task StartSession(AuthorizeUserResponseModel responseModel);

    /// <summary>
    /// Завершить сессию
    /// </summary>
    Task EndSession();
}