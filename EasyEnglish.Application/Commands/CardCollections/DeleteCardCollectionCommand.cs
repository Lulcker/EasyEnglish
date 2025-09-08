using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.CardCollections;

/// <summary>
/// Команда удаления коллекции карточек
/// </summary>
public class DeleteCardCollectionCommand(
    IRepository<Card> cardRepository,
    IRepository<CardCollection> cardCollectionRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    ILogger<DeleteCardCollectionCommand> logger
)
{
    public async Task ExecuteAsync(Guid cardCollectionId)
    {
        var cardCollection = await cardCollectionRepository
            .Include(c => c.Cards)
            .SingleOrDefaultAsync(c => c.Id == cardCollectionId);
        
        cardCollection.ThrowIfNull("Коллекция не найдена");
        
        (cardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();
        
        if (cardCollection.Cards.Count > 0)
            cardRepository.RemoveRange(cardCollection.Cards);
        
        cardCollectionRepository.Remove(cardCollection);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Удалена коллекция {CardCollectionTitle} пользователем с Email: {UserEmail} (Id: {UserId})",
            cardCollection.Title, userInfoProvider.Email, userInfoProvider.Id);
    }
}