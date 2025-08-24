using EasyEnglish.Application.Contracts.Providers;

namespace EasyEnglish.Infrastructure.Providers;

public class HashProvider : IHashProvider
{
    public required byte[] PepperBytes { get; init; }
}