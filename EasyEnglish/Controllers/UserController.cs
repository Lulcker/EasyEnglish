using EasyEnglish.Application.Commands.Users;
using EasyEnglish.Application.Queries.Users;
using EasyEnglish.DTO.Users.RequestModels;
using EasyEnglish.DTO.Users.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyEnglish.Controllers;

/// <summary>
/// Контроллер пользователя
/// </summary>
[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(
    GetReminderSettingsQuery getReminderSettingsQuery,
    SetReminderSettingsCommand setReminderSettingsCommand,
    UpdateReminderSettingsCommand updateReminderSettingsCommand,
    ChangeEnabledReminderSettingsCommand changeEnabledReminderSettingsCommand
    ) : ControllerBase
{
    #region GET

    /// <summary>
    /// Получение настроек напоминаний
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet("reminder-settings")]
    public async Task<ActionResult<ReminderSettingsResponseModel>> GetReminderSettingsAsync(CancellationToken cancellationToken) =>
        Ok(await getReminderSettingsQuery.ExecuteAsync(cancellationToken));

    #endregion
    
    #region POST

    /// <summary>
    /// Установка настроек напоминаний
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPost("set-reminder-settings")]
    public async Task<IActionResult> SetReminderSettingsAsync([FromBody] SetReminderSettingsRequestModel requestModel,
        CancellationToken cancellationToken)
    {
        await setReminderSettingsCommand.ExecuteAsync(requestModel, cancellationToken);
        return Ok();
    }

    #endregion

    #region PATCH
    
    /// <summary>
    /// Обновление настроек напоминаний
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPatch("update-reminder-settings")]
    public async Task<IActionResult> UpdateReminderSettingsAsync(
        [FromBody] UpdateReminderSettingsRequestModel requestModel, CancellationToken cancellationToken)
    {
        await updateReminderSettingsCommand.ExecuteAsync(requestModel, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Переключение работы напоминаний
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPatch("change-enabled-reminder-settings")]
    public async Task<IActionResult> ChangeEnabledReminderSettingsAsync(
        [FromBody] ChangeEnabledReminderSettingsRequestModel requestModel, CancellationToken cancellationToken)
    {
        await changeEnabledReminderSettingsCommand.ExecuteAsync(requestModel, cancellationToken);
        return Ok();
    }

    #endregion
}