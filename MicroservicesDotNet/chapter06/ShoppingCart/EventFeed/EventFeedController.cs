// Exposing events to other microservices

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ShoppingCart.EventFeed;

// (0) Reads the start and end values from a query string parameter
// (1) Returns the raw list of events.
//     MVC takes care of serializing theevents into the response body.

[Route("/events")]
public class EventFeedController : ControllerBase
{
    private readonly IEventStore _eventStore;

    public EventFeedController(IEventStore eventStore) =>
        _eventStore = eventStore;

    [HttpGet("")]
    public Event[] Get(
        [FromQuery] long start, [FromQuery] long end = long.MaxValue)    // (0)
    {
        return _eventStore
            .GetEvents(start, end)    // (1)
            .ToArray();
    }
}
