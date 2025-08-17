namespace EasyEnglish.Core.Domain;

/// <summary>
/// Базовая сущность
/// </summary>
public class EntityBase
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();
}