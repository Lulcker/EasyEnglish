namespace EasyEnglish.Application.Contracts.Services;

public interface IAesCryptoService
{
    /// <summary>
    /// Зашифровать строку
    /// </summary>
    string Encrypt(string plainText);
    
    /// <summary>
    /// Дешифровать строку
    /// </summary>
    string Decrypt(string cipherText);
}