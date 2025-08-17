namespace EasyEnglish.ProxyApiMethods;

/// <summary>
/// Бизнес ошибка
/// </summary>
public class BusinessException(string message) : Exception(message);

/// <summary>
/// Ошибка прав доступа
/// </summary>
public class AccessDeniedException(string message) : Exception(message);

/// <summary>
/// Внутренняя ошибка сервера
/// </summary>
public class InternalServerErrorException(string message) : Exception(message);