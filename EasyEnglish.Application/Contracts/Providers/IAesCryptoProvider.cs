namespace EasyEnglish.Application.Contracts.Providers;

/// <summary>
/// Провайдер AES
/// </summary>
public interface IAesCryptoProvider
{
    /// <summary>
    /// Ключ
    /// </summary>
    byte[] Key { get; }
    
    /// <summary>
    /// Секрет
    /// </summary>
    // ReSharper disable once InconsistentNaming
    byte[] IV { get; }
}