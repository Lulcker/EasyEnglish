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
    ILogger<DeleteCardCollectionCommand> logger
)
{
    public async Task ExecuteAsync(Guid cardCollectionId)
    {
        var cardCollection = await cardCollectionRepository
            .Include(c => c.Cards)
            .SingleOrDefaultAsync(c => c.Id == cardCollectionId);
        
        cardCollection.ThrowIfNull("Коллекция не найдена");
        
        if (cardCollection.Cards.Count > 0)
            cardRepository.RemoveRange(cardCollection.Cards);
        
        cardCollectionRepository.Remove(cardCollection);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Удалена коллекция {CardCollectionTitle}", cardCollection.Title);
    }
}