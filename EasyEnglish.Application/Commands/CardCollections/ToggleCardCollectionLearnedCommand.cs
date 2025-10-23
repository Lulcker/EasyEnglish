using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.CardCollections;

/// <summary>
/// Команда изменения выученности коллекции
/// </summary>
public class ToggleCardCollectionLearnedCommand(
    IRepository<CardCollection> cardCollectionRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    ILogger<ToggleCardCollectionLearnedCommand> logger
    )
{
    public async Task ExecuteAsync(Guid cardCollectionId)
    {
        var cardCollection = await cardCollectionRepository
            .SingleOrDefaultAsync(c => c.Id == cardCollectionId);

        cardCollection.ThrowIfNull("Коллекция не найдена");

        (cardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();

        cardCollection.IsLearned = !cardCollection.IsLearned;

        await unitOfWork.SaveChangesAsync();

        logger.LogInformation(
            "Изменено свойство IsLearned у коллекции с Id: {CardCollectionId}. Новое значение: {IsLearned}. Пользователь: {UserEmail} (Id: {UserId})",
            cardCollectionId, cardCollection.IsLearned, userInfoProvider.Email, userInfoProvider.Id);
    }
}