namespace LoyaltyProgram.EventFeed;

public record EventFeedEvent(
    long SequenceNumber,
    DateTimeOffset OccuredAt,
    string Name,
    object Content);

public interface IEventStore
{
    // Stores events to the event store.
    Task RaiseEvent(string name, object content);
    // Reads events from the event store.
    Task<IEnumerable<EventFeedEvent>> GetEvents(int start, int end);
}

public class EventStore : IEventStore
{
    private static readonly IList<EventFeedEvent> Database =
        new List<EventFeedEvent>();

    private static long _currentSequenceNumber = 0;

    public Task RaiseEvent(string name, object content)
    {
        var seqNumber = Interlocked.Increment(ref _currentSequenceNumber);
        Database.Add(new EventFeedEvent(seqNumber, DateTimeOffset.Now, name, content));
        return Task.CompletedTask;
    }

    public Task<IEnumerable<EventFeedEvent>> GetEvents(int start, int end)
    {
        var result = Database
            .Where(e => start <= e.SequenceNumber && e.SequenceNumber < end)
            .OrderBy(e => e.SequenceNumber);

        return Task.FromResult<IEnumerable<EventFeedEvent>>(result);
    }
}
