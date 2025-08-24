using System.Security.Cryptography;
using System.Text;
using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;

namespace EasyEnglish.Infrastructure.Services;

public class HashService(IHashProvider hashProvider) : IHashService
{
    public string GenerateSalt() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

    public string GenerateHash(string input, string salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(input);
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var passwordWithSaltAndPepperBytes = new byte[passwordBytes.Length + saltBytes.Length + hashProvider.PepperBytes.Length];

        Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltAndPepperBytes, 0, passwordBytes.Length);
        Buffer.BlockCopy(saltBytes, 0, passwordWithSaltAndPepperBytes, passwordBytes.Length, saltBytes.Length);
        Buffer.BlockCopy(hashProvider.PepperBytes, 0, passwordWithSaltAndPepperBytes, passwordBytes.Length + saltBytes.Length, hashProvider.PepperBytes.Length);

        return Convert.ToBase64String(SHA256.HashData(passwordWithSaltAndPepperBytes));
    }
}