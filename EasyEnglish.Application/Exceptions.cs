namespace EasyEnglish.Application;

/// <summary>
/// Бизнес ошибка
/// </summary>
public class BusinessException(string message) : Exception(message);

/// <summary>
/// Ошибка прав доступа
/// </summary>
public class AccessDeniedException(string message = "Недостаточно прав доступа") : Exception(message);

/// <summary>
/// Ошибка для подтверждения действия
/// </summary>
public class ConfirmActionException(string message) : Exception(message);