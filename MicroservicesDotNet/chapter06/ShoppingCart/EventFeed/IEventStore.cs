namespace ShoppingCart.EventFeed;

public interface IEventStore
{
    // Filtering events based on the start and end points
    Task <IEnumerable<Event>> GetEvents(
        long firstEventSequenceNumber, long lastEventSequenceNumber);

    Task Raise(string eventName, object content);
}
