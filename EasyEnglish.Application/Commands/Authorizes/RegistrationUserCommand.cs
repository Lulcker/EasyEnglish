using System.Net.Mail;
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
/// Команда регистрации нового пользователя
/// </summary>
public class RegistrationUserCommand(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IHashService hashService,
    IAesCryptoService aesCryptoService,
    CreateJwtTokenRule createJwtTokenRule,
    ILogger<LoginUserCommand> logger
)
{
    public async Task<AuthorizeUserResponseModel> ExecuteAsync(RegistrationUserRequestModel requestModel)
    {
        var firstName = requestModel.FirstName.Trim().UppercaseFirstLetter();
        var email = requestModel.Email.Trim().ToLower();
        
        firstName.ThrowIfEmpty("Имя не может быть пустым");
        email.ThrowIfEmpty("Почта не может быть пустой");
        
        email.IsValidEmail()
            .ThrowIfInvalidCondition("Неверный адрес электронный почты");
        
        requestModel.Password.ThrowIfEmpty("Пароль не может быть пустым");
        
        (requestModel.Password.Length >= 8)
            .ThrowIfInvalidCondition("Пароль не может быть меньше 8 символов");
        
        (!await userRepository.AnyAsync(u => u.Email == aesCryptoService.Encrypt(email)))
            .ThrowIfInvalidCondition("Пользователь с такой почтой уже существует");
        
        var passwordSalt = hashService.GenerateSalt();

        var newUser = new User
        {
            FirstName = aesCryptoService.Encrypt(firstName),
            Email = aesCryptoService.Encrypt(email),
            PasswordHash = hashService.GenerateHash(requestModel.Password, passwordSalt),
            PasswordSalt = passwordSalt
        };
        
        userRepository.Add(newUser);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Зарегистрирован новый пользователь с Id: {UserId}, Email: {UserEmail}", newUser.Id, email);

        return new AuthorizeUserResponseModel
        {
            Token = createJwtTokenRule.CreateJwtToken(newUser),
            UserId = newUser.Id,
            FirstName = firstName,
            Email = requestModel.Email
        };
    }
}