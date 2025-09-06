using EasyEnglish.DTO.Dictionaries;
using EasyEnglish.DTO.Users.RequestModels;
using EasyEnglish.DTO.Users.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Users;

public partial class ProfilePage(
    UserApiHelper userApiHelper,
    IUserSession userSession,
    IBreadcrumbHelper breadcrumbHelper,
    ISnackbarHelper snackbarHelper
    ) : ComponentBase
{
    #region Consts

    private const int MinPeriodicityTime = 2;

    #endregion
    
    #region Fields

    private bool isDataLoading;

    private ReminderSettingsResponseModel? reminderSettings;

    private bool isEnabledReminderSettings;
    private ReminderMode mode = ReminderMode.Periodicity;
    private TimeSpan? startWorkTime;
    private TimeSpan? endWorkTime;
    private int periodicityTime = MinPeriodicityTime;

    private bool isEditReminderSettings;

    #endregion

    #region Properties

    private bool IsSetReminderSettingsButtonDisabled => !startWorkTime.HasValue || !endWorkTime.HasValue;
    
    private bool IsUpdateReminderSettingsButtonDisabled =>
            !startWorkTime.HasValue ||
            !endWorkTime.HasValue ||
            (startWorkTime == reminderSettings?.StartWorkTime &&
             endWorkTime == reminderSettings?.EndWorkTime &&
             periodicityTime == reminderSettings?.PeriodicityTime);

    #endregion
    
    protected override async Task OnInitializedAsync()
    {
        breadcrumbHelper.SetBreadcrumbs([new BreadcrumbItem("К коллекциям", "/")]);

        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        isDataLoading = true;
        
        reminderSettings = await userApiHelper.GetReminderSettingsAsync();

        if (reminderSettings is not null)
        {
            isEnabledReminderSettings = reminderSettings.IsEnabled;
            mode = reminderSettings.Mode;
            startWorkTime = reminderSettings.StartWorkTime;
            endWorkTime = reminderSettings.EndWorkTime;
            periodicityTime = reminderSettings.PeriodicityTime ?? 0;
        }
        
        isDataLoading = false;
    }

    private async Task StartWorkTimeChanged(TimeSpan? value)
    {
        if (!endWorkTime.HasValue)
        {
            startWorkTime = value;
            return;
        }
        
        if (!value.HasValue)
        {
            startWorkTime = null;
            periodicityTime = MinPeriodicityTime;
            return;
        }
        
        if (value >= endWorkTime)
        {
            await snackbarHelper.ShowError("Значение должно быть меньше времени окончания");
            return;
        }
        
        if (value.Value.Add(TimeSpan.FromHours(2)) > endWorkTime)
        {
            await snackbarHelper.ShowError("Значение должно быть меньше времени окончания минимум на 2 часа");
            return;
        }

        startWorkTime = value;
    }
    
    private async Task EndWorkTimeChanged(TimeSpan? value)
    {
        if (!value.HasValue)
        {
            endWorkTime = null;
            periodicityTime = MinPeriodicityTime;
            return;
        }

        if (value.Value <= startWorkTime)
        {
            await snackbarHelper.ShowError("Значение должно быть больше начального времени");
            return;
        }
        
        if (value.Value.Add(TimeSpan.FromHours(-2)) < startWorkTime)
        {
            await snackbarHelper.ShowError("Значение должно быть больше начального времени минимум на 2 часа");
            return;
        }

        endWorkTime = value;
    }

    private int GetMaxPeriodicityTime()
    {
        if (startWorkTime.HasValue && endWorkTime.HasValue)
            return endWorkTime.Value.Hours - startWorkTime.Value.Hours;

        return 2;
    }

    private void OpenEditReminderSettings()
    {
        isEditReminderSettings = true;
        StateHasChanged();
    }
    
    private void CloseEditReminderSettings()
    {
        isEditReminderSettings = false;
        StateHasChanged();
    }

    private async Task ChangeIsEnabledReminderSettings(bool value)
    {
        isEnabledReminderSettings = value;

        isDataLoading = true;

        try
        {
            await userApiHelper.ChangeEnabledReminderSettingsAsync(new ChangeEnabledReminderSettingsRequestModel
            {
                IsEnabled = isEnabledReminderSettings
            });

            if (isEnabledReminderSettings)
                await snackbarHelper.ShowSuccess("Напоминания успешно включены");
            else
                await snackbarHelper.ShowSuccess("Напоминания успешно выключены");
        }
        finally
        {
            isDataLoading = false;
        }
    }

    private async Task SetOrUpdateReminderSettings()
    {
        isDataLoading = true;

        try
        {
            if (reminderSettings is null)
            {
                await userApiHelper.SetReminderSettingsAsync(new SetReminderSettingsRequestModel
                {
                    Mode = mode,
                    TimeZoneId = TimeZoneInfo.Local.Id,
                    ReminderTimes = null,
                    StartWorkTime = startWorkTime,
                    EndWorkTime = endWorkTime,
                    PeriodicityTime = periodicityTime
                });

                await snackbarHelper.ShowSuccess("Настройки напоминаний успешно установлены");
            }
            else
            {
                await userApiHelper.UpdateReminderSettingsAsync(new UpdateReminderSettingsRequestModel
                {
                    Mode = mode,
                    ReminderTimes = null,
                    StartWorkTime = startWorkTime,
                    EndWorkTime = endWorkTime,
                    PeriodicityTime = periodicityTime
                });
                
                await snackbarHelper.ShowSuccess("Настройки напоминаний успешно обновлены");
            }
            
            CloseEditReminderSettings();

            await LoadDataAsync();
        }
        finally
        {
            isDataLoading = false;
        }
    }
}