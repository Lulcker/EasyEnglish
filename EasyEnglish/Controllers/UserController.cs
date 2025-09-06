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
    [HttpGet("reminder-settings")]
    public async Task<ActionResult<ReminderSettingsResponseModel>> GetReminderSettingsAsync() =>
        Ok(await getReminderSettingsQuery.ExecuteAsync());

    #endregion
    
    #region POST

    /// <summary>
    /// Установка настроек напоминаний
    /// </summary>
    [HttpPost("set-reminder-settings")]
    public async Task<IActionResult> SetReminderSettingsAsync([FromBody] SetReminderSettingsRequestModel requestModel)
    {
        await setReminderSettingsCommand.ExecuteAsync(requestModel);
        return Ok();
    }

    #endregion

    #region PATCH
    
    /// <summary>
    /// Обновление настроек напоминаний
    /// </summary>
    [HttpPatch("update-reminder-settings")]
    public async Task<IActionResult> UpdateReminderSettingsAsync(
        [FromBody] UpdateReminderSettingsRequestModel requestModel)
    {
        await updateReminderSettingsCommand.ExecuteAsync(requestModel);
        return Ok();
    }

    /// <summary>
    /// Переключение работы напоминаний
    /// </summary>
    [HttpPatch("change-enabled-reminder-settings")]
    public async Task<IActionResult> ChangeEnabledReminderSettingsAsync(
        [FromBody] ChangeEnabledReminderSettingsRequestModel requestModel)
    {
        await changeEnabledReminderSettingsCommand.ExecuteAsync(requestModel);
        return Ok();
    }

    #endregion
}