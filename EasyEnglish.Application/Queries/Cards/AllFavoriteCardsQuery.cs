using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.Cards;

/// <summary>
/// Запрос получения избранных карточек пользователя
/// </summary>
public class AllFavoriteCardsQuery(
    IRepository<Card> cardRepository,
    IUserInfoProvider userInfoProvider
)
{
    public async Task<IReadOnlyCollection<CardResponseModel>> ExecuteAsync()
    {
        return await cardRepository
            .AsNoTracking()
            .Where(c => c.CardCollection.UserId == userInfoProvider.Id)
            .Where(c => c.IsFavorite)
            .Select(c => new CardResponseModel
            {
                Id = c.Id,
                RuWord = c.RuWord,
                EnWord = c.EnWord,
                AddedAt = c.AddedAt,
                IsFavorite = c.IsFavorite
            })
            .ToListAsync();
    }
}