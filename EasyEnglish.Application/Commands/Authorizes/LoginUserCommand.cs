using EasyEnglish.Application.Contracts.Services;
using EasyEnglish.Application.Rules.Authorizes;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Authorizes.RequestModels;
using EasyEnglish.DTO.Authorizes.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Authorizes;

/// <summary>
/// Команда входа в систему для пользователя
/// </summary>
public class LoginUserCommand(
    IRepository<User> userRepository,
    IHashService hashService,
    IAesCryptoService aesCryptoService,
    CreateJwtTokenRule createJwtTokenRule,
    ILogger<LoginUserCommand> logger
    )
{
    public async Task<AuthorizeUserResponseModel> ExecuteAsync(LoginUserRequestModel requestModel, CancellationToken cancellationToken)
    {
        var email = requestModel.Email.Trim().ToLower();
        
        email.ThrowIfEmpty("Почта не может быть пустой");
        requestModel.Password.ThrowIfEmpty("Пароль не может быть пустым");
        
        (requestModel.Password.Length >= 8)
            .ThrowIfInvalidCondition("Пароль не может быть меньше 8 символов");
        
        var user = await userRepository
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == aesCryptoService.Encrypt(email),cancellationToken);
        
        user.ThrowIfNull("Пользователь с такой почтой не найден");
        
        user = await userRepository
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == aesCryptoService.Encrypt(email) &&
                                      u.PasswordHash == hashService.GenerateHash(requestModel.Password, user.PasswordSalt), cancellationToken);
        
        user.ThrowIfNull("Неверный пароль");
        
        logger.LogInformation("Пользователь {UserFirstName} Email: {UserEmail} (Id: {UserId}) вошёл в систему",
            aesCryptoService.Decrypt(user.FirstName), aesCryptoService.Decrypt(user.Email), user.Id);

        return new AuthorizeUserResponseModel
        {
            Token = createJwtTokenRule.CreateJwtToken(user),
            UserId = user.Id,
            FirstName = aesCryptoService.Decrypt(user.FirstName),
            Email = aesCryptoService.Decrypt(user.Email)
        };
    }
}