using EasyEnglish.Application.Contracts.Providers;

namespace EasyEnglish.Infrastructure.Providers;

/// <summary>
/// Провайдер URL для UI
/// </summary>
public class UrlUIProvider : IUrlUIProvider
{
    /// <summary>
    /// URL для UI
    /// </summary>
    public required string Url { get; init; }
}