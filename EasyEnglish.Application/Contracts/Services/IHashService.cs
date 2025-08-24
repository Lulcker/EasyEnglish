namespace EasyEnglish.Application.Contracts.Services;

public interface IHashService
{
    string GenerateSalt();
    
    string GenerateHash(string input, string salt);
}