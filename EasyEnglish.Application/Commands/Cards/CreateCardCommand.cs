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

        var card = await cardRepository
            .SingleOrDefaultAsync(c => c.CardCollectionId == requestModel.CardCollectionId &&
                                       c.RuWord.ToLower() == requestModel.RuWord.Trim().ToLower());
        
        card.ThrowIfNotNull("Карточка с таким словом уже существует в этой коллекции");

        card = new Card
        {
            RuWord = requestModel.RuWord.Trim().UppercaseFirstLetter(),
            EnWord = requestModel.EnWord.Trim().UppercaseFirstLetter(),
            AddedAt = DateTime.UtcNow,
            CardCollectionId = requestModel.CardCollectionId
        };
        
        cardRepository.Add(card);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Добавлена карточка {RuWord} - {EnWord} в коллекцию {CardCollectionTitle}",
            card.RuWord, card.EnWord, cardCollection.Title);
    }
}