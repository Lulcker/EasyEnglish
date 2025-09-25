using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Cards;

/// <summary>
/// Команда создания карточки
/// </summary>
public class CreateCardCommand(
    IRepository<Card> cardRepository,
    IRepository<CardCollection> cardCollectionRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    ILogger<CreateCardCommand> logger
)
{
    public async Task ExecuteAsync(CreateCardRequestModel requestModel)
    {
        requestModel.RuWord.ThrowIfEmpty("Не написано русское слово");
        
        requestModel.EnWord.ThrowIfEmpty("Не написано английское слово");
        
        var cardCollection = await cardCollectionRepository
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == requestModel.CardCollectionId);
        
        cardCollection.ThrowIfNull("Коллекция не найдена");
        
        (cardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();

        var card = await cardRepository
            .SingleOrDefaultAsync(c => c.CardCollectionId == requestModel.CardCollectionId &&
                                       c.RuWord.ToLower() == requestModel.RuWord.Trim().ToLower());
        
        card.ThrowIfNotNull("Карточка с таким словом уже существует в этой коллекции");
        
        if (!requestModel.IsConfirmAction)
        {
            var existsCardsByRuWord = await cardRepository
                .AsNoTracking()
                .Include(c => c.CardCollection)
                .Where(c => c.CardCollection.UserId == userInfoProvider.Id &&
                            c.RuWord.ToLower() == requestModel.RuWord.Trim().ToLower())
                .ToListAsync();
            
            if (existsCardsByRuWord.Count > 0)
                false.ThrowConfirmActionIfInvalidCondition(GetConfirmText(existsCardsByRuWord));
        }

        card = new Card
        {
            RuWord = requestModel.RuWord.Trim().UppercaseFirstLetter(),
            EnWord = requestModel.EnWord.Trim().UppercaseFirstLetter(),
            AddedAt = DateTime.UtcNow,
            CardCollectionId = requestModel.CardCollectionId
        };
        
        cardRepository.Add(card);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Добавлена карточка {RuWord} - {EnWord} в коллекцию {CardCollectionTitle}  пользователем с Email: {UserEmail} (Id: {UserId})",
            card.RuWord, card.EnWord, cardCollection.Title, userInfoProvider.Email, userInfoProvider.Id);
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