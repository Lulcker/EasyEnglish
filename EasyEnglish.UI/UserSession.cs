using System.Security.Claims;
using Blazored.LocalStorage;
using EasyEnglish.DTO.Authorizes.ResponseModels;
using EasyEnglish.UI.Contracts;
using EasyEnglish.UI.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EasyEnglish.UI;

public class UserSession(
    ILocalStorageService localStorageService,
    NavigationManager navigationManager
    ) : AuthenticationStateProvider, IUserSession
{
    #region Consts

    private const string TokenKey = "easy-english.token";
    private const string FirstNameKey = "easy-english.first-name";
    private const string EmailKey = "easy-english.email";

    #endregion

    #region Properties

    public string Token { get; set; } = null!;
    
    public string FirstName { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    #endregion

    #region Fields

    private bool isSessionStarted;

    #endregion

    #region Methods
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await LoadSession();

        if (isSessionStarted)
        {
            var authorize = new ClaimsIdentity("Authorize");
            return new AuthenticationState(new ClaimsPrincipal(authorize));
        }

        var anonymous = new ClaimsIdentity();
        return new AuthenticationState(new ClaimsPrincipal(anonymous));
    }

    public async Task StartSession(AuthorizeUserResponseModel responseModel)
    {
        await localStorageService.SetItemAsync(TokenKey, responseModel.Token);
        await localStorageService.SetItemAsync(FirstNameKey, responseModel.FirstName);
        await localStorageService.SetItemAsync(EmailKey, responseModel.Email);
        
        navigationManager.NavigateTo("/");
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task EndSession()
    {
        await localStorageService.RemoveItemAsync(TokenKey);
        await localStorageService.RemoveItemAsync(FirstNameKey);
        await localStorageService.RemoveItemAsync(EmailKey);
        
        navigationManager.NavigateTo("/sign-in");
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    #endregion

    #region Private Methods

    private async Task LoadSession()
    {
        if ((await localStorageService.GetItemAsStringAsync(TokenKey)).IsNotEmpty())
        {
            await LoadUserData();
            isSessionStarted = true;
        }
        else
        {
            ClearUserData();
            isSessionStarted = false;
        }
    }
    
    private async Task LoadUserData()
    {
        Token = await GetStringItemAsync(TokenKey);
        FirstName = await GetStringItemAsync(FirstNameKey);
        Email = await GetStringItemAsync(EmailKey);
    }

    private void ClearUserData()
    {
        Token = null!;
        FirstName = null!;
        Email = null!;
    }
    
    private async Task<string> GetStringItemAsync(string key) =>
        (await localStorageService.GetItemAsStringAsync(key))!.Trim('"');

    #endregion
}