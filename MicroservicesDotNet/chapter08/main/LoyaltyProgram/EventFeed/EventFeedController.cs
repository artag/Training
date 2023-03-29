using Microsoft.AspNetCore.Mvc;

namespace LoyaltyProgram.EventFeed;

[Route("/events")]
public class EventFeedController : ControllerBase
{
    private readonly IEventStore _eventStore;

    public EventFeedController(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    // Gets the start and end value from the query string.
    [HttpGet("")]
    public async Task<ActionResult<EventFeedEvent[]>> GetEvents(
        [FromQuery] int start, [FromQuery] int end)
    {
        if (start < 0 || end < start)
            return BadRequest();

        // Reads events "start" through "end" from the event store.
        var events = await _eventStore.GetEvents(start, end);
        return events.ToArray();
    }
}
