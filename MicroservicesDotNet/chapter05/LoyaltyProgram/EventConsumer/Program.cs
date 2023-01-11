// (1) Read the starting point of this batch (пакета) from a database.
// (2) Send GET request to the event feed.
// (3) Call the method to process the events in this batch.
//     ProcessEvents also updates the start variable.
// (4) Save the starting point of the next batch of events.

using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

var start = await GetStartIdFromDatastore();    // (1)
var end = 100;

var client = new HttpClient();
client.DefaultRequestHeaders
    .Accept
    .Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

using var resp = await client.GetAsync(         // (2)
    new Uri($"http://special-offers:5002/events?start={start}&end={end}"));

await ProcessEvents(await resp.Content.ReadAsStreamAsync());    // (3)
await SaveStartIdToDataStore(start);                            // (4)

// Fake implementation. Should get from a real database
Task<long> GetStartIdFromDatastore() => Task.FromResult(0L);

// Fake implementation. Should save to a real database
Task SaveStartIdToDataStore(long startId) => Task.CompletedTask;

// Fake implementation. Should apply business rules to events
// (5) This is where the event would be processed.
// (6) Keeps track of the highest event number handled.
async Task ProcessEvents(Stream content)
{
    var events = await JsonSerializer.DeserializeAsync<SpecialOfferEvent[]>(content)
        ?? Array.Empty<SpecialOfferEvent>();
    foreach (var @event in events)
    {
        Console.WriteLine(@event);                              // (5)
        start = Math.Max(start, @event.SequenceNumber + 1);     // (6)
    }
}

public record SpecialOfferEvent(long SequenceNumber, DateTimeOffset OccuredAt, string Name, object Content);
