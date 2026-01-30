using EasyEnglish.Application.Contracts.Providers;
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
    IUserInfoProvider userInfoProvider,
    ILogger<UpdateCardCollectionCommand> logger
    )
{
    public async Task ExecuteAsync(UpdateCardCollectionRequestModel requestModel, CancellationToken cancellationToken)
    {
        var cardCollection = await cardCollectionRepository
            .SingleOrDefaultAsync(c => c.Id == requestModel.Id, cancellationToken);
        
        cardCollection.ThrowIfNull("Коллекция не найдена");
        
        (cardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();
        
        var existsCardCollectionByTitle = await cardCollectionRepository
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Title.Trim().ToLower() == requestModel.Title.Trim().ToLower(), cancellationToken);
        
        existsCardCollectionByTitle.ThrowIfNotNull("Коллекция с таким названием уже существует");

        var oldCardCollectionTitle = cardCollection.Title;
        
        cardCollection.Title = requestModel.Title.Trim().UppercaseFirstLetter();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Обновлена коллекция Id: {CardCollectionId} пользователем с Email: {UserEmail} (Id: {UserId}), Название {OldCardCollectionTitle} -> {NewCardCollectionTitle}",
            cardCollection.Id, userInfoProvider.Email, userInfoProvider.Id, oldCardCollectionTitle, cardCollection.Title);
    }
}