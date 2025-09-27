using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Cards;

/// <summary>
/// Команда перемещения карточки в другую коллекцию
/// </summary>
public class MoveCardCommand(
    IRepository<Card> cardRepository,
    IRepository<CardCollection> cardCollectionRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    ILogger<MoveCardCommand> logger
    )
{
    public async Task ExecuteAsync(MoveCardRequestModel requestModel)
    {
        var card = await cardRepository
            .Include(c => c.CardCollection)
            .SingleOrDefaultAsync(c => c.Id == requestModel.CardId);
        
        card.ThrowIfNull("Карточка не найдена");
        
        (card.CardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();
        
        (card.CardCollectionId != requestModel.CardCollectionId)
            .ThrowIfInvalidCondition("Карточка уже находится в этой коллекции");

        var cardCollection = await cardCollectionRepository
            .AsNoTracking()
            .Include(c => c.Cards)
            .SingleOrDefaultAsync(c => c.Id == requestModel.CardCollectionId);
        
        cardCollection.ThrowIfNull("Коллекция не найдена");
        
        (cardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();
        
        cardCollection.Cards.All(c => !string.Equals(c.RuWord, card.RuWord, StringComparison.CurrentCultureIgnoreCase))
            .ThrowIfInvalidCondition("Карточка с таким словом уже есть в этой коллекции");

        var oldCardCollectionTitle = card.CardCollection.Title;
        
        card.CardCollectionId = requestModel.CardCollectionId;

        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Перемещена карточка {CardId} из коллекции {OldCardCollectionTitle} в коллекцию {NewCardCollectionTitle}",
            card.Id, oldCardCollectionTitle, cardCollection.Title);
    }
}