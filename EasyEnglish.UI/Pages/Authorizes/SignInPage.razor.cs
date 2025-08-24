using EasyEnglish.DTO.Authorizes.RequestModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;

namespace EasyEnglish.UI.Pages.Authorizes;

/// <summary>
/// Страница входа в аккаунт
/// </summary>
public partial class SignInPage(
    AuthorizeApiHelper authorizeApiHelper,
    IUserSession userSession
    ) : ComponentBase
{
    #region Fields
    
    private string email = string.Empty;
    
    private string password = string.Empty;
    
    private string firstName = string.Empty;
    
    private string repeatPassword = string.Empty;
    
    private bool isLoading;

    private bool isLogin = true;
    
    #endregion
    
    #region Properties
    
    private bool IsLoginButtonDisabled => string.IsNullOrEmpty(email) ||
                                          string.IsNullOrEmpty(password) ||
                                          isLoading;
    
    private bool IsRegistrationButtonDisabled => string.IsNullOrEmpty(firstName) ||
                                                 string.IsNullOrEmpty(email) ||
                                                 string.IsNullOrEmpty(password) ||
                                                 string.IsNullOrEmpty(repeatPassword) ||
                                                 password != repeatPassword ||
                                                 isLoading;
    
    #endregion
    
    #region Methods

    private void IsLoginChange(bool value)
    {
        isLogin = value;
        
        email = string.Empty;
        password = string.Empty;
        firstName = string.Empty;
        repeatPassword = string.Empty;
        
        StateHasChanged();
    }

    private async Task LoginAsync()
    {
        isLoading = true;

        try
        {
            var result = await authorizeApiHelper.LoginAsync(new LoginUserRequestModel
            {
                Email = email.Trim(),
                Password = password
            });

            await userSession.StartSession(result);
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }
    
    private async Task RegistrationAsync()
    {
        isLoading = true;

        try
        {
            var result = await authorizeApiHelper.RegistrationAsync(new RegistrationUserRequestModel
            {
                FirstName = firstName.Trim(),
                Email = email.Trim(),
                Password = password
            });

            await userSession.StartSession(result);
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    #endregion
}