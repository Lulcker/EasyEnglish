using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.CardCollections;

/// <summary>
/// Запрос получения списка коллекций карточек
/// </summary>
public class AllCardCollectionsQuery(IRepository<CardCollection> cardCollectionRepository)
{
    public async Task<IReadOnlyCollection<CardCollectionResponseModel>> ExecuteAsync()
    {
        return await cardCollectionRepository
            .AsNoTracking()
            .Select(c => new CardCollectionResponseModel
            {
                Id = c.Id,
                Title = c.Title,
                CreatedAt = c.CreatedAt,
                CardsCount = c.Cards.Count
            })
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}