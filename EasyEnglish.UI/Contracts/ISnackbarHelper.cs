namespace EasyEnglish.UI.Contracts;

/// <summary>
/// Помощник всплывающего окна
/// </summary>
public interface ISnackbarHelper
{
    /// <summary>
    /// Показать успешное сообщение
    /// </summary>
    /// <param name="message">Сообщение</param>
    ValueTask ShowSuccess(string message);

    /// <summary>
    /// Показать сообщение с информацией
    /// </summary>
    /// <param name="message">Сообщение</param>
    ValueTask ShowInfo(string message);

    /// <summary>
    /// Показать сообщение с ошибкой
    /// </summary>
    /// <param name="message">Сообщение</param>
    ValueTask ShowError(string message);
    
    /// <summary>
    /// Показать сообщение с подтверждением
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="header">Заголовок</param>
    /// <param name="confirmButtonText">Текст для кнопки подтверждения</param>
    /// <param name="cancelButtonText">Текст для кнопки отмены</param>
    ValueTask<bool> ShowConfirm(
        string message,
        string header = "Предупреждение",
        string confirmButtonText = "Да",
        string cancelButtonText = "Нет"
    );
    
    /// <summary>
    /// Показать сообщение с предупреждением
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="header">Заголовок</param>
    ValueTask ShowWarningMessageBox(
        string message,
        string header = "Внимание"
    );
}