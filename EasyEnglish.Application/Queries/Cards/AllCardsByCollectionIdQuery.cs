using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.Cards;

/// <summary>
/// Запрос получения списка карточек в коллекции
/// </summary>
public class AllCardsByCollectionIdQuery(
    IRepository<Card> cardRepository,
    IRepository<CardCollection> cardCollectionRepository,
    IUserInfoProvider userInfoProvider
    )
{
    public async Task<IReadOnlyCollection<CardResponseModel>> ExecuteAsync(Guid cardCollectionId, CancellationToken cancellationToken)
    {
        var cardCollectionUserId = await cardCollectionRepository
            .AsNoTracking()
            .Where(c => c.Id == cardCollectionId)
            .Select(c => c.UserId)
            .SingleOrDefaultAsync(cancellationToken);
        
        (cardCollectionUserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();
        
        return await cardRepository
            .AsNoTracking()
            .Where(c => c.CardCollectionId == cardCollectionId)
            .Select(c => new CardResponseModel
            {
                Id = c.Id,
                RuWord = c.RuWord,
                EnWord = c.EnWord,
                AddedAt = c.AddedAt,
                IsFavorite = c.IsFavorite
            })
            .OrderByDescending(c => c.AddedAt)
            .ToListAsync(cancellationToken);
    }
}