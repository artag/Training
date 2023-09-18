namespace TableSort.Infrastructure;

/// <summary>
/// Критерии сортировки.
/// </summary>
public enum SortStatus
{
    /// <summary>
    /// По идентификатору по возрастанию.
    /// </summary>
    IdAsc,

    /// <summary>
    /// По идентификатору по убыванию.
    /// </summary>
    IdDesc,

    /// <summary>
    /// По имени по возрастанию.
    /// </summary>
    FirstNameAsc,

    /// <summary>
    /// По имени по убыванию.
    /// </summary>
    FirstNameDesc,

    /// <summary>
    /// По фамилии по возрастанию.
    /// </summary>
    LastNameAsc,

    /// <summary>
    /// По фамилии по убыванию.
    /// </summary>
    LastNameDesc,

    /// <summary>
    /// По дате рождения по возрастанию.
    /// </summary>
    BirthDateAsc,

    /// <summary>
    /// По дате рождения по убыванию.
    /// </summary>
    BirthDateDesc,
}
