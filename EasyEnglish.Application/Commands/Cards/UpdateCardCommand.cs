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
        
        var existsCard = await cardRepository
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.CardCollectionId == card.CardCollectionId &&
                                       c.RuWord.ToLower() == requestModel.RuWord.Trim().ToLower());

        (existsCard is null || existsCard.Id == card.Id)
            .ThrowIfInvalidCondition("Карточка с таким словом уже существует в этой коллекции");
        
        if (!requestModel.IsConfirmAction)
        {
            var existsCardsByRuWord = await cardRepository
                .AsNoTracking()
                .Include(c => c.CardCollection)
                .Where(c => c.CardCollection.UserId == userInfoProvider.Id &&
                                          c.RuWord.ToLower() == requestModel.RuWord.Trim().ToLower())
                .ToListAsync();
            
            if (existsCardsByRuWord.Count > 0)
                string.Equals(card.RuWord, requestModel.RuWord, StringComparison.CurrentCultureIgnoreCase)
                    .ThrowConfirmActionIfInvalidCondition(GetConfirmText(existsCardsByRuWord));
        }

        var oldRuWord = card.RuWord;
        var oldEnWord = card.EnWord;
        
        card.RuWord = requestModel.RuWord.Trim().UppercaseFirstLetter();
        card.EnWord = requestModel.EnWord.Trim().UppercaseFirstLetter();

        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Обновлена карточка {OldRuWord} -> {NewRuWord}, {OldEnWord} -> {NewEnWord} пользователем с Email: {UserEmail} (Id: {UserId})",
            oldRuWord, card.RuWord, oldEnWord, card.EnWord, userInfoProvider.Email, userInfoProvider.Id);
    }

    #region Private Methods

    private static string GetConfirmText(List<Card> cards)
    {
        if (cards.Count > 1)
            return "В других коллекциях уже есть такое слово. Вы хотите добавить слово в эту коллекцию?";
        
        var card = cards.First();

        return $"В коллекции '{card.CardCollection.Title}' уже есть такое слово с переводом '{card.EnWord}'. Вы хотите добавить слово в эту коллекцию?";
    }

    #endregion
}