using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.CardCollections;

/// <summary>
/// Запрос получения списка коллекций карточек
/// </summary>
public class AllCardCollectionsQuery(
    IRepository<CardCollection> cardCollectionRepository,
    IUserInfoProvider userInfoProvider
    )
{
    public async Task<IReadOnlyCollection<CardCollectionResponseModel>> ExecuteAsync()
    {
        return await cardCollectionRepository
            .AsNoTracking()
            .Where(c => c.UserId == userInfoProvider.Id)
            .Select(c => new CardCollectionResponseModel
            {
                Id = c.Id,
                Title = c.Title,
                CreatedAt = c.CreatedAt,
                IsLearned = c.IsLearned,
                CardsCount = c.Cards.Count
            })
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}