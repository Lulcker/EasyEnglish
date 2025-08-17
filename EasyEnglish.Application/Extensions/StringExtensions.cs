namespace EasyEnglish.Application.Extensions;

/// <summary>
/// Расширения для String
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Является ли строка пустой
    /// </summary>
    /// <param name="str">Строка</param>
    internal static bool IsEmpty(this string? str) =>
        string.IsNullOrWhiteSpace(str);
    
    /// <summary>
    /// Является ли строка не пустой
    /// </summary>
    /// <param name="str">Строка</param>
    internal static bool IsNotEmpty(this string? str) =>
        !string.IsNullOrWhiteSpace(str);
    
    /// <summary>
    /// Преобразование первой буквы в заглавную
    /// </summary>
    /// <param name="str">Строка</param>
    internal static string UppercaseFirstLetter(this string str) =>
        string.Concat(str[0].ToString().ToUpperInvariant() + str[1..]);
}