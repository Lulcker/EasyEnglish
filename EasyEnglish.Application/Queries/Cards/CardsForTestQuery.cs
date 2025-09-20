using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.DTO.Dictionaries;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.Cards;

/// <summary>
/// Запрос получения карточек для теста
/// </summary>
public class CardsForTestQuery(
    IRepository<CardCollection> cardCollectionRepository,
    IRepository<Card> cardRepository,
    IUserInfoProvider userInfoProvider
    )
{
    public async Task<IReadOnlyCollection<CardForTestResponseModel>> ExecuteAsync(CardForTestRequestModel requestModel)
    {
        List<CardForTestResponseModel> cards;
        
        if (requestModel.CardCollectionId.HasValue)
        {
            var cardCollectionUserId = await cardCollectionRepository
                .AsNoTracking()
                .Where(c => c.Id == requestModel.CardCollectionId.Value)
                .Select(c => c.UserId)
                .SingleOrDefaultAsync();

            (cardCollectionUserId == userInfoProvider.Id)
                .ThrowAccessIfInvalidCondition();

            cards = await cardRepository
                .AsNoTracking()
                .Where(c => c.CardCollectionId == requestModel.CardCollectionId)
                .OrderBy(_ => Guid.NewGuid())
                .Select(c => new CardForTestResponseModel
                {
                    Id = c.Id,
                    RuWord = c.RuWord,
                    EnWord = c.EnWord,
                    AddedAt = c.AddedAt,
                    IsFavorite = c.IsFavorite
                })
                .ToListAsync();
        }
        else
        {
            cards = await cardRepository
                .AsNoTracking()
                .Where(c => c.CardCollection.UserId == userInfoProvider.Id)
                .OrderBy(_ => Guid.NewGuid())
                .Select(c => new CardForTestResponseModel
                {
                    Id = c.Id,
                    RuWord = c.RuWord,
                    EnWord = c.EnWord,
                    AddedAt = c.AddedAt,
                    IsFavorite = c.IsFavorite
                })
                .Take(25)
                .ToListAsync();

            cards = [..cards.DistinctBy(c => c.EnWord).Take(20)];
        }
        
        switch (requestModel)
        {
            case { UseAnswerChoice: true, UseAnswerWriting: false }:
                break;
            case { UseAnswerChoice: false, UseAnswerWriting: true }:
                cards.ForEach(c => c.Level = CardLevel.Three);
                break;
            case { UseAnswerChoice: true, UseAnswerWriting: true }:
                foreach (var card in cards.OrderBy(_ => Guid.NewGuid()).Take(cards.Count / 2))
                    card.Level = CardLevel.Three;
                break;
        }

        return cards;
    }
}