using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.Cards;

/// <summary>
/// Запрос получения карточек для поиска
/// </summary>
public class SearchCardsQuery(
    IRepository<Card> cardRepository,
    IUserInfoProvider userInfoProvider
    )
{
    public async Task<IReadOnlyCollection<SearchCardResponseModel>> ExecuteAsync(string searchText)
    {
        var query = cardRepository
            .AsNoTracking()
            .Where(c => c.CardCollection.UserId == userInfoProvider.Id);

        if (searchText.IsNotEmpty())
            query = query.Where(c => c.RuWord.ToLower().StartsWith(searchText.ToLower()) ||
                                     c.EnWord.ToLower().StartsWith(searchText.ToLower()));

        return await query
            .Select(c => new SearchCardResponseModel
            {
                Id = c.Id,
                RuWord = c.RuWord,
                EnWord = c.EnWord,
                CardCollectionId = c.CardCollectionId,
                CardCollectionTitle = c.CardCollection.Title
            })
            .Take(5)
            .ToListAsync();
    }
}