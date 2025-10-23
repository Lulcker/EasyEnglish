using EasyEnglish.Application.Contracts.Providers;
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
    IUserInfoProvider userInfoProvider,
    ILogger<DeleteCardCommand> logger
    )
{
    public async Task ExecuteAsync(Guid cardId)
    {
        var card = await cardRepository
            .Include(c => c.CardCollection)
            .SingleOrDefaultAsync(c => c.Id == cardId);
        
        card.ThrowIfNull("Карточка не найдена");
        
        (card.CardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();
        
        cardRepository.Remove(card);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Карточка с Id: {CardId} удалена пользователем с Email: {UserEmail} (Id: {UserId})",
            cardId, userInfoProvider.Email, userInfoProvider.Id);
    }
}