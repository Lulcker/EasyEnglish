using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.CardCollections.RequestModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.CardCollections;

/// <summary>
/// Команда создания коллекции карточек
/// </summary>
public class CreateCardCollectionCommand(
    IRepository<CardCollection> cardCollectionRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    ILogger<CreateCardCollectionCommand> logger
    )
{
    public async Task<Guid> ExecuteAsync(CreateCardCollectionRequestModel requestModel)
    {
        requestModel.Title.ThrowIfEmpty("Название коллекции не должно быть пустым");

        var cardCollection = await cardCollectionRepository
            .SingleOrDefaultAsync(c => c.Title.Trim().ToLower() == requestModel.Title.Trim().ToLower());
        
        cardCollection.ThrowIfNotNull("Коллекция с таким названием уже существует");

        cardCollection = new CardCollection
        {
            Title = requestModel.Title.Trim().UppercaseFirstLetter(),
            CreatedAt = DateTime.UtcNow,
            UserId = userInfoProvider.Id
        };

        cardCollectionRepository.Add(cardCollection);
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Создана коллекция {CardCollectionTitle} пользователем с Email: {UserEmail}",
            cardCollection.Title, userInfoProvider.Email);

        return cardCollection.Id;
    }
}