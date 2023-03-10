using System.Net;
using System.Text.Json;
using LoyaltyProgram.EventFeed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace LoyaltyProgramUnitTests;

public class EventFeed_should : IDisposable
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _sut;

    // (1) - Configures what's registered in the service collection.
    // (2) - Registers FakeEventStore as the implementation of IEventStore.
    // (3) - Limits the host to using EventFeedController only.
    public EventFeed_should()
    {
        _app = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b =>
            {
                b.ConfigureServices(s => s                                  // (1)
                    .AddScoped<IEventStore, FakeEventStore>()               // (2)
                    .AddControllersByType(typeof(EventFeedController))      // (3)
                    .AddApplicationPart(typeof(EventFeedController).Assembly));
                b.Configure(x => x
                    .UseRouting()
                    .UseEndpoints(opt => opt.MapControllers()));
                b.UseTestServer();
            });

        _sut = _app.CreateClient();
    }

    [Fact]
    public async Task return_events_when_from_event_store()
    {
        // Act
        var actual = await _sut.GetAsync("/events?start=0&end=100");

        // Assert
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);

        var content = await actual.Content.ReadAsStreamAsync();
        var events = await JsonSerializer.DeserializeAsync<IEnumerable<EventFeedEvent>>(content)
                     ?? Enumerable.Empty<EventFeedEvent>();
        Assert.Equal(100, events.Count());
    }

    [Fact]
    public async Task return_empty_response_when_there_are_no_more_events()
    {
        // Act
        var actual = await _sut.GetAsync("/events?start=200&end=300");

        // Assert
        var content = await actual.Content.ReadAsStreamAsync();
        var events = await JsonSerializer.DeserializeAsync<IEnumerable<EventFeedEvent>>(content);
        Assert.NotNull(events);
        Assert.Empty(events);
    }

    public void Dispose()
    {
        _app?.Dispose();
        _sut?.Dispose();
    }
}
