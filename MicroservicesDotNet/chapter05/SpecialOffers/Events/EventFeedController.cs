using Microsoft.AspNetCore.Mvc;

namespace SpecialOffers.Events;

[Route("/events")]
public class EventFeedController : ControllerBase
{
    private readonly IEventStore _eventStore;

    public EventFeedController(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    [HttpGet("")]
    public ActionResult<EventFeedEvent[]> GetEvents(
        [FromQuery] int start, [FromQuery] int end)
    {
        if (start < 0 || end < start)
            return BadRequest();

        return _eventStore.GetEvents(start, end).ToArray();
    }
}
