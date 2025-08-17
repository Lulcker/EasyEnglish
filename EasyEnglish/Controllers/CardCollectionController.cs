using Microsoft.AspNetCore.Mvc;
using EasyEnglish.Application.Commands.CardCollections;
using EasyEnglish.Application.Queries.CardCollections;
using EasyEnglish.DTO.CardCollections.RequestModels;
using EasyEnglish.DTO.CardCollections.ResponseModels;

namespace EasyEnglish.Controllers;

/// <summary>
/// Контроллер коллекций карточек
/// </summary>
[ApiController]
[Route("api/card-collection")]
public class CardCollectionController(
    CardCollectionByIdQuery cardCollectionByIdQuery,
    AllCardCollectionsQuery allCardCollectionsQuery,
    CreateCardCollectionCommand createCardCollectionCommand,
    UpdateCardCollectionCommand updateCardCollectionCommand,
    DeleteCardCollectionCommand deleteCardCollectionCommand
    ) : ControllerBase
{
    #region GET
    
    /// <summary>
    /// Получение коллекции карточки по Id
    /// </summary>
    /// <param name="id">Id коллекции</param>
    /// <returns>Коллекция карточек?</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CardCollectionResponseModel?>> GetByIdAsync([FromRoute] Guid id) =>
        Ok(await cardCollectionByIdQuery.ExecuteAsync(id));

    /// <summary>
    /// Получение списка коллекций карточек
    /// </summary>
    /// <returns>Список коллекций карточек</returns>
    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyCollection<CardCollectionResponseModel>>> AllAsync() =>
        Ok(await allCardCollectionsQuery.ExecuteAsync());

    #endregion

    #region POST

    /// <summary>
    /// Создание коллекции
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <returns>Id коллекции</returns>
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateCardCollectionRequestModel requestModel) =>
        Ok(await createCardCollectionCommand.ExecuteAsync(requestModel));

    #endregion

    #region PATCH

    /// <summary>
    /// Обновление коллекции
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateCardCollectionRequestModel requestModel)
    {
        await updateCardCollectionCommand.ExecuteAsync(requestModel);
        return Ok();
    }

    #endregion

    #region DELETE

    /// <summary>
    /// Удаление коллекции
    /// </summary>
    /// <param name="cardCollectionId">ID коллекции</param>
    [HttpDelete("delete/{cardCollectionId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid cardCollectionId)
    {
        await deleteCardCollectionCommand.ExecuteAsync(cardCollectionId);
        return Ok();
    }

    #endregion
}