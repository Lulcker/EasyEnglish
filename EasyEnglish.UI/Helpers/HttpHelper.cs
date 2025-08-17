using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using EasyEnglish.DTO;
using EasyEnglish.ProxyApiMethods;

namespace EasyEnglish.UI.Helpers;

/// <summary>
/// Помощник HTTP запросов
/// </summary>
public class HttpHelper(
    HttpClient httpClient
    ) : IHttpHelper
{
    #region Json seriliaze options

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    #endregion

    #region Methods

    public async Task<TResponse> SendAsync<TRequest, TResponse>(string path, HttpMethod method, TRequest requestModel)
    {
        var request = new HttpRequestMessage(method, path);
        request.Content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");

        var response = await SendAsync(request);
        
        if (response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<TResponse>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
        
        await ThrowExceptionAsync(response);

        return default!;
    }
    
    public async Task SendAsync<TRequest>(string path, HttpMethod method, TRequest requestModel)
    {
        var request = new HttpRequestMessage(method, path);
        request.Content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
            
        var response = await SendAsync(request);

        if (response.IsSuccessStatusCode)
            return;
        
        await ThrowExceptionAsync(response);
    }
    
    public async Task<TResponse> SendAsync<TResponse>(string path, HttpMethod method)
    {
        var response = await SendAsync(new HttpRequestMessage(method, path));
        
        if (response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<TResponse>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
        
        await ThrowExceptionAsync(response);

        return default!;
    }
    
    public async Task SendAsync(string path, HttpMethod method)
    {
        var response = await SendAsync(new HttpRequestMessage(method, path));
        
        if (response.IsSuccessStatusCode)
            return;

        await ThrowExceptionAsync(response);
    }

    #endregion

    #region Private methods

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
    {
        HttpResponseMessage? response;
        
        try
        {
            response = await httpClient.SendAsync(requestMessage);
        }
        catch (Exception)
        {
            throw new InternalServerErrorException("Внутренняя ошибка сервера");
        }

        return response;
    }

    private async Task ThrowExceptionAsync(HttpResponseMessage response)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.Forbidden:
                throw new AccessDeniedException("Доступ запрещён");
            case HttpStatusCode.BadRequest:
                throw new BusinessException(JsonSerializer.Deserialize<ErrorResponseModel>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!.Error);
            case HttpStatusCode.InternalServerError:
                throw new InternalServerErrorException("Внутренняя ошибка сервера");
            case HttpStatusCode.Unauthorized:
                //await userSession.EndSession();
                break;
            default:
                throw new BusinessException("Произошла неожиданная ошибка");
        }
    }

    #endregion
}