﻿// (1) Read the starting point of this batch (пакета) from a database.
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
// (1) All events were assumed to be successfully handled, and "start"
//     was updated for each one.
async Task ProcessEvents(Stream content)
{
    var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    var events = await JsonSerializer.DeserializeAsync<SpecialOfferEvent[]>(content, options)
        ?? Array.Empty<SpecialOfferEvent>();

    foreach (var ev in events)
    {
        dynamic eventData = ev.Content;
        if (ShouldSendNotification(eventData))
            await SendNotification(eventData);

        start = ev.SequenceNumber + 1;         // (1)
    }
}

// Decide if notification should be sent based on business rules.
bool ShouldSendNotification(dynamic eventData) =>
    true;

// Use HttpClient to send command to notification microservice.
Task SendNotification(dynamic eventData) =>
    Task.CompletedTask;

public record SpecialOfferEvent(long SequenceNumber, DateTimeOffset OccuredAt, string Name, object Content);
