using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.CardCollections;

/// <summary>
/// Запрос получения коллекции карточек по Id
/// </summary>
public class CardCollectionByIdQuery(IRepository<CardCollection> cardCollectionRepository)
{
    public async Task<CardCollectionResponseModel?> ExecuteAsync(Guid id)
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
            .SingleOrDefaultAsync(c => c.Id == id);
    }
}