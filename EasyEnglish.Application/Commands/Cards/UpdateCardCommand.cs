using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Cards;

/// <summary>
/// Команда обновления карточки
/// </summary>
public class UpdateCardCommand(
    IRepository<Card> cardRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    ILogger<UpdateCardCommand> logger
)
{
    public async Task ExecuteAsync(UpdateCardRequestModel requestModel)
    {
        requestModel.RuWord.ThrowIfEmpty("Не написано русское слово");
        
        requestModel.EnWord.ThrowIfEmpty("Не написано английское слово");

        var card = await cardRepository
            .Include(c => c.CardCollection)
            .SingleOrDefaultAsync(c => c.Id == requestModel.Id);
        
        card.ThrowIfNull("Карточка не найдена");
        
        (card.CardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();

        var oldRuWord = card.RuWord;
        var oldEnWord = card.EnWord;
        
        card.RuWord = requestModel.RuWord.Trim().UppercaseFirstLetter();
        card.EnWord = requestModel.EnWord.Trim().UppercaseFirstLetter();

        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Обновлена карточка {OldRuWord} -> {NewRuWord}, {OldEnWord} -> {NewEnWord}  пользователем с Email: {UserEmail} (Id: {UserId})",
            oldRuWord, card.RuWord, oldEnWord, card.EnWord, userInfoProvider.Email, userInfoProvider.Id);
    }
}