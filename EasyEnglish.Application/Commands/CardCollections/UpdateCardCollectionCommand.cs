using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.CardCollections.RequestModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.CardCollections;

/// <summary>
/// Команда обновления коллекции карточек
/// </summary>
public class UpdateCardCollectionCommand(
    IRepository<CardCollection> cardCollectionRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateCardCollectionCommand> logger
)
{
    public async Task ExecuteAsync(UpdateCardCollectionRequestModel requestModel)
    {
        var cardCollection = await cardCollectionRepository
            .SingleOrDefaultAsync(c => c.Id == requestModel.Id);
        
        cardCollection.ThrowIfNull("Коллекция не найдена");
        
        var existsCardCollectionByTitle = await cardCollectionRepository
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Title.Trim().ToLower() == requestModel.Title.Trim().ToLower());
        
        existsCardCollectionByTitle.ThrowIfNotNull("Коллекция с таким названием уже существует");

        var oldCardCollectionTitle = cardCollection.Title;
        
        cardCollection.Title = requestModel.Title.Trim().UppercaseFirstLetter();

        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Обновлена коллекция Id: {CardCollectionId}, Название {OldCardCollectionTitle} -> {NewCardCollectionTitle}",
            cardCollection.Id, oldCardCollectionTitle, cardCollection.Title);
    }
}