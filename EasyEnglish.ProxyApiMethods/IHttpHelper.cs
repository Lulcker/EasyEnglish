namespace EasyEnglish.ProxyApiMethods;

/// <summary>
/// Помощник HTTP запросов
/// </summary>
public interface IHttpHelper
{
    /// <summary>
    /// Метод выполнения запроса
    /// </summary>
    /// <param name="path">Путь запроса</param>
    /// <param name="method">HTTP метод</param>
    /// <param name="requestModel">Входная модель</param>
    /// <typeparam name="TRequest">Тип входной модели</typeparam>
    /// <typeparam name="TResponse">Тип модели результата</typeparam>
    /// <returns>Результат</returns>
    Task<TResponse> SendAsync<TRequest, TResponse>(string path, HttpMethod method, TRequest requestModel, CancellationToken cancellationToken);
    
    /// <summary>
    /// Метод выполнения запроса
    /// </summary>
    /// <param name="path">Путь запроса</param>
    /// <param name="method">HTTP метод</param>
    /// <param name="requestModel">Входная модель</param>
    /// <typeparam name="TRequest">Тип входной модели</typeparam>
    Task SendAsync<TRequest>(string path, HttpMethod method, TRequest requestModel, CancellationToken cancellationToken);
    
    /// <summary>
    /// Метод выполнения запроса
    /// </summary>
    /// <param name="path">Путь запроса</param>
    /// <param name="method">HTTP метод</param>
    /// <typeparam name="TResponse">Тип модели результата</typeparam>
    /// <returns>Результат</returns>
    Task<TResponse> SendAsync<TResponse>(string path, HttpMethod method, CancellationToken cancellationToken);
    
    /// <summary>
    /// Метод выполнения запроса
    /// </summary>
    /// <param name="path">Путь запроса</param>
    /// <param name="method">HTTP метод</param>
    Task SendAsync(string path, HttpMethod method, CancellationToken cancellationToken);
}