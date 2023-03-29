using System.Net;
using Microsoft.AspNetCore.TestHost;
using Sut;

namespace SutTests;

public class TestController_should : IDisposable
{
    private readonly IHost _host;
    private readonly HttpClient _sut;

    // (1) - Create an ASP.NET host for the test endpoint.
    // (2) - Explicitly add the test controller to the service collection
    //       using a custom extension method.
    // (3) - Map all endpoints in the test controller.
    public TestController_should()
    {
        _host = new HostBuilder()           // (1)
            .ConfigureWebHost(host =>
                host.ConfigureServices(services =>
                    services.AddControllersByType(typeof(TestController)))  // (2)
            .Configure(appBuilder =>
                appBuilder
                    .UseRouting()
                    .UseEndpoints(opt => opt.MapControllers()))     // (3)
            .UseTestServer())
        .Start();

        _sut = _host.GetTestClient();
    }

    // (1) - Call the endpoint in the test controller.
    // (2) - Assert that the call succeeded.
    [Fact]
    public async Task respond_ok_to_request_to_root()
    {
        var actual = await _sut.GetAsync("/");                  // (1)
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);     // (2)
    }

    // (1) - Stop and dispose the ASP.NET host after the test is done.
    public void Dispose()
    {
        _host?.Dispose();       // (1)
        _sut?.Dispose();
    }
}
