using Prism.Events;

namespace Demo.Infrastructure
{
    public class PersonUpdatedEvent : PubSubEvent<string>
    {
    }
}
