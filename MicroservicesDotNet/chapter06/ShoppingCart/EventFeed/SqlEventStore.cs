using System.Data.SqlClient;
using System.Text.Json;
using Dapper;

namespace ShoppingCart.EventFeed;

public class SqlEventStore : IEventStore
{
    private const string ConnectionString =
        @"Data Source=localhost;Initial Catalog=ShoppingCart;User Id=SA; Password=Some_password!";

    private const string WriteEventSql = @"
INSERT INTO EventStore(Name, OccuredAt, Content)
VALUES (@Name, @OccuredAt, @Content)";

    private const string ReadEventsSql = @"
SELECT * FROM EventStore
WHERE ID >= @Start AND ID <= @End";

    // (0) - Uses Dapper to execute a simple SQL insert statement.
    public async Task Raise(string eventName, object content)
    {
        var jsonContent = JsonSerializer.Serialize(content);
        await using var conn = new SqlConnection(ConnectionString);
        await conn.ExecuteAsync(                    // (0)
            WriteEventSql,
            new
            {
                Name = eventName,
                OccuredAt = DateTimeOffset.Now,
                Content = jsonContent
            });
    }

    // (1) - Maps EventStore table rows to Event objects.
    // (2) - Reads EventStore table rows between start and end
    public async Task<IEnumerable<Event>> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
        await using var conn = new SqlConnection(ConnectionString);
        return await conn.QueryAsync<Event>(            // (1)
            ReadEventsSql,
            new
            {
                Start = firstEventSequenceNumber,       // (2)
                End = lastEventSequenceNumber
            });
    }
}
