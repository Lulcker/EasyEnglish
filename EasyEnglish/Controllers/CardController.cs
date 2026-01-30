using Microsoft.AspNetCore.Mvc;
using EasyEnglish.Application.Commands.Cards;
using EasyEnglish.Application.Queries.Cards;
using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using Microsoft.AspNetCore.Authorization;

namespace EasyEnglish.Controllers;

/// <summary>
/// Контроллер карточек
/// </summary>
[Authorize]
[ApiController]
[Route("api/card")]
public class CardController(
    AllCardsByCollectionIdQuery allCardsByCollectionIdQuery,
    AllFavoriteCardsQuery allFavoriteCardsQuery,
    CardsForTestQuery cardsForTestQuery,
    SearchCardsQuery searchCardsQuery,
    CreateCardCommand createCardCommand,
    UpdateCardCommand updateCardCommand,
    ToggleCardFavoriteCommand toggleCardFavoriteCommand,
    MoveCardCommand moveCardCommand,
    DeleteCardCommand deleteCardCommand
    ) : ControllerBase
{
    #region GET

    /// <summary>
    /// Получение списка карточек в коллекции
    /// </summary>
    /// <param name="cardCollectionId">Id коллекции карточек</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список карточек в коллекции</returns>
    [HttpGet("all-by-collection/{cardCollectionId:guid}")]
    public async Task<ActionResult<IReadOnlyCollection<CardResponseModel>>> AllByCollectionIdAsync([FromRoute] Guid cardCollectionId, CancellationToken cancellationToken) =>
        Ok(await allCardsByCollectionIdQuery.ExecuteAsync(cardCollectionId, cancellationToken));

    /// <summary>
    /// Получение списка избранных карточек
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список избранных карточек</returns>
    [HttpGet("all-favorite")]
    public async Task<ActionResult<IReadOnlyCollection<CardResponseModel>>> AllFavoriteAsync(CancellationToken cancellationToken) =>
        Ok(await allFavoriteCardsQuery.ExecuteAsync(cancellationToken));

    /// <summary>
    /// Получение карточек в поиске
    /// </summary>
    /// <param name="searchText">Текст поиска</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список карточек</returns>
    [HttpGet("search-cards")]
    public async Task<ActionResult<IReadOnlyCollection<SearchCardResponseModel>>> SearchCardsAsync([FromQuery] string searchText, CancellationToken cancellationToken) =>
        Ok(await searchCardsQuery.ExecuteAsync(searchText, cancellationToken));

    #endregion

    #region POST
    
    /// <summary>
    /// Получение списка карточек для теста
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список карточек в коллекции</returns>
    [HttpPost("for-test")]
    public async Task<ActionResult<IReadOnlyCollection<CardForTestResponseModel>>> GetForTestAsync([FromBody] CardForTestRequestModel requestModel, CancellationToken cancellationToken) =>
        Ok(await cardsForTestQuery.ExecuteAsync(requestModel, cancellationToken));

    /// <summary>
    /// Создание карточки
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCardRequestModel requestModel, CancellationToken cancellationToken)
    {
        await createCardCommand.ExecuteAsync(requestModel, cancellationToken);
        return Ok();
    }

    #endregion

    #region PATCH

    /// <summary>
    /// Обновление карточки
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateCardRequestModel requestModel, CancellationToken cancellationToken)
    {
        await updateCardCommand.ExecuteAsync(requestModel, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Изменение избранности карточки
    /// </summary>
    /// <param name="cardId">Id карточки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPatch("toggle-favorite/{cardId:guid}")]
    public async Task<IActionResult> ToggleFavoriteAsync([FromRoute] Guid cardId, CancellationToken cancellationToken)
    {
        await toggleCardFavoriteCommand.ExecuteAsync(cardId, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Перемещение карточки в другую коллекцию
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPatch("move")]
    public async Task<IActionResult> MoveAsync([FromBody] MoveCardRequestModel requestModel, CancellationToken cancellationToken)
    {
        await moveCardCommand.ExecuteAsync(requestModel, cancellationToken);
        return Ok();
    }

    #endregion

    #region DELETE

    /// <summary>
    /// Удаление карточки
    /// </summary>
    /// <param name="cardId">Id карточки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpDelete("delete/{cardId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid cardId, CancellationToken cancellationToken)
    {
        await deleteCardCommand.ExecuteAsync(cardId, cancellationToken);
        return Ok();
    }

    #endregion
}