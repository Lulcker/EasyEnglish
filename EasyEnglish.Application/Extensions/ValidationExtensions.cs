using System.Diagnostics.CodeAnalysis;

namespace EasyEnglish.Application.Extensions;

/// <summary>
/// Расширения для валидации
/// </summary>
internal static class ValidationExtensions
{
    /// <summary>
    /// Выкидывает ошибку, если значение - пустая строка
    /// </summary>
    /// <param name="value">Значение</param>
    /// <param name="message">Сообщение ошибки</param>
    public static void ThrowIfEmpty(this string value, string message)
    {
        if (value.IsEmpty())
            throw new BusinessException(message);
    }

    /// <summary>
    /// Выкидывает ошибку, если значение Null
    /// </summary>
    /// <param name="value">Значение</param>
    /// <param name="message">Сообщение ошибки</param>
    public static void ThrowIfNull<T>([NotNull] this T? value, string message) where T : class
    {
        if (value is null)
            throw new BusinessException(message);
    }
    
    /// <summary>
    /// Выкидывает ошибку, если значение не Null
    /// </summary>
    /// <param name="value">Значение</param>
    /// <param name="message">Сообщение ошибки</param>
    public static void ThrowIfNotNull<T>(this T? value, string message) where T : class
    {
        if (value is not null)
            throw new BusinessException(message);
    }

    /// <summary>
    /// Выкидывает ошибку, если выражение неверное
    /// </summary>
    /// <param name="condition">Выражение</param>
    /// <param name="message"></param>
    public static void ThrowIfInvalidCondition(this bool condition, string message)
    {
        if (!condition)
            throw new BusinessException(message);
    }
}