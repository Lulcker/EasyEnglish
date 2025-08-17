using EasyEnglish.Domain.Entities;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Cards;

/// <summary>
/// Команда удаления карточки
/// </summary>
public class DeleteCardCommand(
    IRepository<Card> cardRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteCardCommand> logger
)
{
    public async Task ExecuteAsync(Guid cardId)
    {
        var card = await cardRepository
            .SingleOrDefaultAsync(c => c.Id == cardId);
        
        card.ThrowIfNull("Карточка не найдена");
        
        cardRepository.Remove(card);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Карточка с Id: {CardId} удалена", cardId);
    }
}