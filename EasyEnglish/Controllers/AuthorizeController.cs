using EasyEnglish.Application.Commands.Authorizes;
using EasyEnglish.DTO.Authorizes.RequestModels;
using EasyEnglish.DTO.Authorizes.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace EasyEnglish.Controllers;

/// <summary>
/// Контроллер авторизации
/// </summary>
[ApiController]
[Route("api/authorize")]
public class AuthorizeController(
    LoginUserCommand  loginUserCommand,
    RegistrationUserCommand registrationUserCommand
    ) : ControllerBase
{
    #region POST

    /// <summary>
    /// Вход пользователя в систему
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <returns>Данные авторизации</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthorizeUserResponseModel>> LoginAsync([FromBody] LoginUserRequestModel requestModel) =>
        Ok(await loginUserCommand.ExecuteAsync(requestModel));
    
    /// <summary>
    /// Регистрация пользователя в системе
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <returns>Данные авторизации</returns>
    [HttpPost("registration")]
    public async Task<ActionResult<AuthorizeUserResponseModel>> RegistrationAsync([FromBody] RegistrationUserRequestModel requestModel) =>
        Ok(await registrationUserCommand.ExecuteAsync(requestModel));

    #endregion
}