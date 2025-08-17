namespace EasyEnglish.DTO;

/// <summary>
/// Модель ошибки
/// </summary>
public class ErrorResponseModel
{
    /// <summary>
    /// Ошибка
    /// </summary>
    public required string Error { get; init; }
}