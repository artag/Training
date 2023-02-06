namespace ShoppingCart.EventFeed;

// Для mapping'а в Dapper:
// 1. Наименования свойств должны совпадать со столбцами в БД.
// 2. Типы данных должны совпадать с типами данных в столбцах БД.
public record Event(
    long Id,
    string Name,
    DateTimeOffset OccuredAt,
    string Content);
