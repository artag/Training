using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using LoyaltyProgram.Users;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace LoyaltyProgramUnitTests;

// (1) - Real LoyaltyProgram startup.
// (2) - Use the test server so requests are in process.
// (3) - The host uses the test server to create a test HttpClient.
public class UserEndpoints_should : IDisposable
{
    // private readonly IHost _host;  // .NET 5.0
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _sut;

    public UserEndpoints_should()
    {
        //// For NET 5.0
        // _host = new HostBuilder()
        //     .ConfigureWebHost(x => x
        //         .UseStartup<Program>()                   // (1)
        //         .UseTestServer())                        // (2)
        //     .Start();
        //
        // _sut = _host.GetTestClient();                    // (3)

        // For NET 6.0
        _app = new WebApplicationFactory<Program>()         // (1)
            .WithWebHostBuilder(x => x.UseTestServer());    // (2)

        _sut = _app.CreateClient();                         // (3)
    }

    [Fact]
    public async Task respond_not_found_when_queried_for_unregistered_user()
    {
        var actual = await _sut.GetAsync("/users/1000");
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task allow_to_register_new_user()
    {
        var expected = new LoyaltyProgramUser(
            Id: 0, Name: "Christian", LoyaltyPoints: 0, new LoyaltyProgramSettings());

        // Registers a new user through the POST endpoint.
        var content = new StringContent(
            JsonSerializer.Serialize(expected),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var registrationResponse = await _sut.PostAsync("/users", content);

        // Reads the new user from the body of the response from the POST.
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var contentStream = await registrationResponse.Content.ReadAsStreamAsync();
        var newUser = await JsonSerializer.DeserializeAsync<LoyaltyProgramUser>(
            contentStream, options);

        // Reads the new user through the GET endpoint.
        var actual = await _sut.GetAsync($"/users/{newUser?.Id}");
        var contentString = await actual.Content.ReadAsStringAsync();
        var actualUser = JsonSerializer.Deserialize<LoyaltyProgramUser>(
            contentString, options);

        // Checks that the response from the GET is correct.
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(expected.Name, actualUser?.Name);
    }

    [Fact]
    public async Task allow_modifying_users()
    {
        var expected = "jane";
        var user = new LoyaltyProgramUser(
            Id: 0, Name: "Christian", LoyaltyPoints: 0, new LoyaltyProgramSettings());

        // Registers a user.
        var postContent = new StringContent(
            JsonSerializer.Serialize(user),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var registrationResponse = await _sut.PostAsync("/users", postContent);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var newUser = await JsonSerializer.DeserializeAsync<LoyaltyProgramUser>(
            await registrationResponse.Content.ReadAsStreamAsync(),
            options);

        // Updates the user.
        var updatedUser = newUser! with { Name = expected };
        var putContent = new StringContent(
            JsonSerializer.Serialize(updatedUser),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var actual = await _sut.PutAsync($"/users/{newUser.Id}", putContent);

        var actualUser = await JsonSerializer.DeserializeAsync<LoyaltyProgramUser>(
            await actual.Content.ReadAsStreamAsync(),
            options);

        // Asserts that the update was done.
        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equal(expected, actualUser?.Name);
    }

    public void Dispose()
    {
        //// For NET 5.0
        // _host?.Dispose();
        // _sut?.Dispose();

        // For NET 6.0
        _app?.Dispose();
        _sut?.Dispose();
    }
}
