using Microsoft.AspNetCore.Mvc;
using EasyEnglish.Application.Commands.CardCollections;
using EasyEnglish.Application.Queries.CardCollections;
using EasyEnglish.DTO.CardCollections.RequestModels;
using EasyEnglish.DTO.CardCollections.ResponseModels;
using Microsoft.AspNetCore.Authorization;

namespace EasyEnglish.Controllers;

/// <summary>
/// Контроллер коллекций карточек
/// </summary>
[Authorize]
[ApiController]
[Route("api/card-collection")]
public class CardCollectionController(
    CardCollectionByIdQuery cardCollectionByIdQuery,
    AllCardCollectionsQuery allCardCollectionsQuery,
    CreateCardCollectionCommand createCardCollectionCommand,
    UpdateCardCollectionCommand updateCardCollectionCommand,
    ToggleCardCollectionLearnedCommand toggleCardCollectionLearnedCommand,
    DeleteCardCollectionCommand deleteCardCollectionCommand
    ) : ControllerBase
{
    #region GET
    
    /// <summary>
    /// Получение коллекции карточки по Id
    /// </summary>
    /// <param name="id">Id коллекции</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция карточек?</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CardCollectionResponseModel?>> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
        Ok(await cardCollectionByIdQuery.ExecuteAsync(id, cancellationToken));

    /// <summary>
    /// Получение списка коллекций карточек
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список коллекций карточек</returns>
    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyCollection<CardCollectionResponseModel>>> AllAsync(CancellationToken cancellationToken) =>
        Ok(await allCardCollectionsQuery.ExecuteAsync(cancellationToken));

    #endregion

    #region POST

    /// <summary>
    /// Создание коллекции
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Id коллекции</returns>
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateCardCollectionRequestModel requestModel, CancellationToken cancellationToken) =>
        Ok(await createCardCollectionCommand.ExecuteAsync(requestModel, cancellationToken));

    #endregion

    #region PATCH

    /// <summary>
    /// Обновление коллекции
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateCardCollectionRequestModel requestModel, CancellationToken cancellationToken)
    {
        await updateCardCollectionCommand.ExecuteAsync(requestModel, cancellationToken);
        return Ok();
    }
    
    /// <summary>
    /// Изменение избранности карточки
    /// </summary>
    /// <param name="cardCollectionId">Id коллекции</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPatch("toggle-learned/{cardCollectionId:guid}")]
    public async Task<IActionResult> ToggleLearnedAsync([FromRoute] Guid cardCollectionId, CancellationToken cancellationToken)
    {
        await toggleCardCollectionLearnedCommand.ExecuteAsync(cardCollectionId, cancellationToken);
        return Ok();
    }

    #endregion

    #region DELETE

    /// <summary>
    /// Удаление коллекции
    /// </summary>
    /// <param name="cardCollectionId">ID коллекции</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpDelete("delete/{cardCollectionId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid cardCollectionId, CancellationToken cancellationToken)
    {
        await deleteCardCollectionCommand.ExecuteAsync(cardCollectionId, cancellationToken);
        return Ok();
    }

    #endregion
}