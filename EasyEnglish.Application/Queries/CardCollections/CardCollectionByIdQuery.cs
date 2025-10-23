using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.CardCollections;

/// <summary>
/// Запрос получения коллекции карточек по Id
/// </summary>
public class CardCollectionByIdQuery(
    IRepository<CardCollection> cardCollectionRepository,
    IUserInfoProvider userInfoProvider
    )
{
    public async Task<CardCollectionResponseModel?> ExecuteAsync(Guid id)
    {
        var cardCollection = await cardCollectionRepository
            .AsNoTracking()
            .Include(c => c.Cards)
            .SingleOrDefaultAsync(c => c.Id == id);
        
        cardCollection.ThrowIfNull("Коллекция не найдена");
        
        (cardCollection.UserId == userInfoProvider.Id)
            .ThrowAccessIfInvalidCondition();

        return new CardCollectionResponseModel
        {
            Id = cardCollection.Id,
            Title = cardCollection.Title,
            CreatedAt = cardCollection.CreatedAt,
            IsLearned = cardCollection.IsLearned,
            CardsCount = cardCollection.Cards.Count
        };
    }
}