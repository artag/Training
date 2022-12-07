namespace ShoppingCart.EventFeed;

// (0) Gets a sequence number for the event

public interface IEventStore
{
    // Filtering events based on the start and end points
    IEnumerable<Event> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber);

    void Raise(string eventName, object content);
}

public class EventStore : IEventStore
{
    private static long _currentSequenceNumber = 0;
    private static readonly IList<Event> Database = new List<Event>();

    // Filtering events based on the start and end points
    public IEnumerable<Event> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber) =>
        Database
            .Where(e =>
                e.SequenceNumber >= firstEventSequenceNumber &&
                e.SequenceNumber <= lastEventSequenceNumber)
            .OrderBy(e => e.SequenceNumber);

    public void Raise(string eventName, object content)
    {
        var seqNumber = Interlocked.Increment(ref _currentSequenceNumber);
        var ev = new Event(seqNumber, DateTimeOffset.UtcNow, eventName, content);
        Database.Add(ev);
    }
}
