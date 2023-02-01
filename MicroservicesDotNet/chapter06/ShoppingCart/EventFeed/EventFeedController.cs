// Exposing events to other microservices

using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.EventFeed;

// (0) Reads the start and end values from a query string parameter
// (1) Returns the raw list of events.
//     MVC takes care of serializing the events into the response body.

[Route("/events")]
public class EventFeedController : ControllerBase
{
    private readonly IEventStore _eventStore;

    public EventFeedController(IEventStore eventStore) =>
        _eventStore = eventStore;

    [HttpGet("")]
    public async Task<Event[]> Get(
        [FromQuery] long start, [FromQuery] long end = long.MaxValue)   // (0)
    {
        return (await _eventStore.GetEvents(start, end)).ToArray();     // (1)
    }
}
