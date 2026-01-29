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
    /// <returns>Список карточек в коллекции</returns>
    [HttpGet("all-by-collection/{cardCollectionId:guid}")]
    public async Task<ActionResult<IReadOnlyCollection<CardResponseModel>>> AllByCollectionIdAsync([FromRoute] Guid cardCollectionId) =>
        Ok(await allCardsByCollectionIdQuery.ExecuteAsync(cardCollectionId));

    /// <summary>
    /// Получение списка избранных карточек
    /// </summary>
    /// <returns>Список избранных карточек</returns>
    [HttpGet("all-favorite")]
    public async Task<ActionResult<IReadOnlyCollection<CardResponseModel>>> AllFavoriteAsync() =>
        Ok(await allFavoriteCardsQuery.ExecuteAsync());

    /// <summary>
    /// Получение карточек в поиске
    /// </summary>
    /// <param name="searchText">Текст поиска</param>
    /// <returns>Список карточек</returns>
    [HttpGet("search-cards")]
    public async Task<ActionResult<IReadOnlyCollection<SearchCardResponseModel>>> SearchCardsAsync([FromQuery] string searchText) =>
        Ok(await searchCardsQuery.ExecuteAsync(searchText));

    #endregion

    #region POST
    
    /// <summary>
    /// Получение списка карточек для теста
    /// </summary>
    /// <returns>Список карточек в коллекции</returns>
    [HttpPost("for-test")]
    public async Task<ActionResult<IReadOnlyCollection<CardForTestResponseModel>>> GetForTestAsync([FromBody] CardForTestRequestModel requestModel) =>
        Ok(await cardsForTestQuery.ExecuteAsync(requestModel));

    /// <summary>
    /// Создание карточки
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCardRequestModel requestModel)
    {
        await createCardCommand.ExecuteAsync(requestModel);
        return Ok();
    }

    #endregion

    #region PATCH

    /// <summary>
    /// Обновление карточки
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateCardRequestModel requestModel)
    {
        await updateCardCommand.ExecuteAsync(requestModel);
        return Ok();
    }

    /// <summary>
    /// Изменение избранности карточки
    /// </summary>
    /// <param name="cardId">Id карточки</param>
    [HttpPatch("toggle-favorite/{cardId:guid}")]
    public async Task<IActionResult> ToggleFavoriteAsync([FromRoute] Guid cardId)
    {
        await toggleCardFavoriteCommand.ExecuteAsync(cardId);
        return Ok();
    }

    /// <summary>
    /// Перемещение карточки в другую коллекцию
    /// </summary>
    /// <param name="requestModel">Входная модель</param>
    [HttpPatch("move")]
    public async Task<IActionResult> MoveAsync([FromBody] MoveCardRequestModel requestModel)
    {
        await moveCardCommand.ExecuteAsync(requestModel);
        return Ok();
    }

    #endregion

    #region DELETE

    /// <summary>
    /// Удаление карточки
    /// </summary>
    /// <param name="cardId">Id карточки</param>
    [HttpDelete("delete/{cardId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid cardId)
    {
        await deleteCardCommand.ExecuteAsync(cardId);
        return Ok();
    }

    #endregion
}