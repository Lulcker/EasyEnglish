using System.Security.Cryptography;
using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;

namespace EasyEnglish.Infrastructure.Services;

public class AesCryptoService(IAesCryptoProvider aesCryptoProvider) : IAesCryptoService
{
    public string Encrypt(string plainText)
    {
        var aesAlg = Aes.Create();
        
        aesAlg.Key = aesCryptoProvider.Key;
        aesAlg.IV = aesCryptoProvider.IV;
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }

        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        var aesAlg = Aes.Create();
        
        aesAlg.Key = aesCryptoProvider.Key;
        aesAlg.IV = aesCryptoProvider.IV;
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        
        return srDecrypt.ReadToEnd();
    }
}