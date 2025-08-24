using EasyEnglish.Application.Contracts.Providers;

namespace EasyEnglish.Infrastructure.Providers;

public class AesCryptoProvider : IAesCryptoProvider
{
    public required byte[] Key { get; init; }
    
    public required byte[] IV { get; init; }
}