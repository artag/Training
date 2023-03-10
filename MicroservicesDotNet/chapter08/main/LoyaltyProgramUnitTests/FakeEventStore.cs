using LoyaltyProgram.EventFeed;

namespace LoyaltyProgramUnitTests;

public class FakeEventStore : IEventStore
{
    public Task RaiseEvent(string name, object content) =>
        throw new NotImplementedException();

    public Task<IEnumerable<EventFeedEvent>> GetEvents(int start, int end)
    {
        // Returns an empty list when start is more than 100.
        if (start > 100)
            return Task.FromResult(Enumerable.Empty<EventFeedEvent>());

        // Returns a list of fake events when start is less than 100.
        var result = Enumerable
            .Range(start, end - start)
            .Select(i => new EventFeedEvent(
                SequenceNumber: i,
                OccuredAt: DateTimeOffset.Now,
                Name: "some event",
                Content: new object()));

        return Task.FromResult(result);
    }
}
