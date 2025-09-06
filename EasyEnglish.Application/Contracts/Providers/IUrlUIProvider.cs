namespace EasyEnglish.Application.Contracts.Providers;

/// <summary>
/// Провайдер URL для UI
/// </summary>
public interface IUrlUIProvider
{
    /// <summary>
    /// URL для UI
    /// </summary>
    public string Url { get; }
}