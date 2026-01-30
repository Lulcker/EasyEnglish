using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Cards;

/// <summary>
/// Команда изменения избранности карточки
/// </summary>
public class ToggleCardFavoriteCommand(
    IRepository<Card> cardRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    ILogger<ToggleCardFavoriteCommand> logger
    )
{
    public async Task ExecuteAsync(Guid cardId, CancellationToken cancellationToken)
    {
        var card = await cardRepository
            .Include(c => c.CardCollection)
            .SingleOrDefaultAsync(c => c.Id == cardId, cancellationToken);
        
        card.ThrowIfNull("Карточка не найдена");
        
        (card.CardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();

        card.IsFavorite = !card.IsFavorite;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Изменено свойство IsFavorite у карточки с Id: {CardId}. Новое значение: {IsFavorite}. Пользователь: {UserEmail} (Id: {UserId})",
            cardId, card.IsFavorite, userInfoProvider.Email, userInfoProvider.Id);
    }
}