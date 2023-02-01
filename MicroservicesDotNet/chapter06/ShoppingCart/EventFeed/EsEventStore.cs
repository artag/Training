using System.Text;
using System.Text.Json;
using EventStore.ClientAPI;

namespace ShoppingCart.EventFeed;

public class EsEventStore : IEventStore
{
    private const string ConnectionString =
        "tcp://admin:changeit@localhost:1113";

    // (1) - For local development only.In production, TLS should be enabled.
    // (2) - Creates a connection to EventStore.
    // (3) - Opens the connection to EventStore.
    // (4) - Writes the event to EventStore.
    // (5) - EventData is EventStore's representation of an event.
    // (6) - Maps OccurredAt and EventName to metadata to be stored along with the event.
    public async Task Raise(string eventName, object content)
    {
        var settings = ConnectionSettings.Create().DisableTls().Build();            // (1)
        var uri = new Uri(ConnectionString);
        using var connection = EventStoreConnection.Create(settings, uri);          // (2)

        await connection.ConnectAsync();                                            // (3)

        var jsonContent = JsonSerializer.Serialize(content);
        var data = Encoding.UTF8.GetBytes(jsonContent);

        var eventMetadata = new EventMetadata(DateTimeOffset.UtcNow, eventName);    // (6)
        var jsonMetadata = JsonSerializer.Serialize(eventMetadata);
        var metadata = Encoding.UTF8.GetBytes(jsonMetadata);

        var events = new[]
        {
            new EventData(                                  // (5)
                eventId: Guid.NewGuid(),
                type: "ShoppingCartEvent",
                isJson: true,
                data: data,
                metadata: metadata)
        };

        await connection.ConditionalAppendToStreamAsync(    // (4)
            stream: "ShoppingCart",
            expectedVersion: ExpectedVersion.Any,
            events: events);
    }

    // (7) - Reads events from the EventStoreDB.
    // (8) - Accesses the events on the result from the EventStoreDB.
    // (9) - Gets the content part of each event.
    // (10) - Gets the metadata part of each event.
    // (11) - Maps to events from EventStoreDB Event objects.
    public async Task<IEnumerable<Event>> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
        var settings = ConnectionSettings.Create().DisableTls().Build();
        var uri = new Uri(ConnectionString);
        using var connection = EventStoreConnection.Create(settings, uri);

        await connection.ConnectAsync();

        var count = (int)(lastEventSequenceNumber - firstEventSequenceNumber);
        var result = await connection.ReadStreamEventsForwardAsync(     // (7)
            stream: "ShoppingCart",
            start: firstEventSequenceNumber,
            count: count,
            resolveLinkTos: false);

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        return result.Events        // (8)
            .Select(e =>
                new
                {
                    Content = Encoding.UTF8.GetString(e.Event.Data),            // (9)
                    Metadata = JsonSerializer.Deserialize<EventMetadata>(       // (10)
                        Encoding.UTF8.GetString(e.Event.Metadata), options)
                })
            .Select((e, i) =>       // (11)
                new Event(
                    SequenceNumber: i + firstEventSequenceNumber,
                    OccuredAt: e.Metadata.OccuredAt,
                    Name: e.Metadata.EventName,
                    Content: e.Content));
    }
}

public record EventMetadata(DateTimeOffset OccuredAt, string EventName);
