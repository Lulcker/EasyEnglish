using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.Cards;

/// <summary>
/// Запрос получения списка карточек в коллекции
/// </summary>
public class AllCardsByCollectionIdQuery(IRepository<Card> cardCollectionRepository)
{
    public async Task<IReadOnlyCollection<CardResponseModel>> ExecuteAsync(Guid cardCollectionId)
    {
        return await cardCollectionRepository
            .AsNoTracking()
            .Where(c => c.CardCollectionId == cardCollectionId)
            .Select(c => new CardResponseModel
            {
                Id = c.Id,
                RuWord = c.RuWord,
                EnWord = c.EnWord,
                AddedAt = c.AddedAt
            })
            .OrderByDescending(c => c.AddedAt)
            .ToListAsync();
    }
}