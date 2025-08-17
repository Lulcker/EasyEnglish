using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Cards;

/// <summary>
/// Команда обновления карточки
/// </summary>
public class UpdateCardCommand(
    IRepository<Card> cardRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateCardCommand> logger
)
{
    public async Task ExecuteAsync(UpdateCardRequestModel requestModel)
    {
        requestModel.RuWord.ThrowIfEmpty("Не написано русское слово");
        
        requestModel.EnWord.ThrowIfEmpty("Не написано английское слово");

        var card = await cardRepository
            .SingleOrDefaultAsync(c => c.Id == requestModel.Id);
        
        card.ThrowIfNull("Карточка не найдена");

        var oldRuWord = card.RuWord;
        var oldEnWord = card.EnWord;
        
        card.RuWord = requestModel.RuWord.Trim().UppercaseFirstLetter();
        card.EnWord = requestModel.EnWord.Trim().UppercaseFirstLetter();

        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Обновлена карточка {OldRuWord} -> {NewRuWord}, {OldEnWord} -> {NewEnWord}",
            oldRuWord, card.RuWord, oldEnWord, card.EnWord);
    }
}