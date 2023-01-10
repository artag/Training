using System.Text;
using System.Text.Json;

namespace ApiGatewayMock;

public class LoyaltyProgramClient
{
    private readonly HttpClient _httpClient;

    public LoyaltyProgramClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // (1) Sends the command to loyalty program
    public Task<HttpResponseMessage> RegisterUser(string name)
    {
        var user = new { name, Settings = new { } };
        return _httpClient.PostAsync("/users/", CreateBody(user));      // (1)
    }

    // (1) Sends the UpdateUser command as a PUT request
    public Task<HttpResponseMessage> UpdateUser(LoyaltyProgramUser user) =>
        _httpClient.PutAsync($"/users/{user.Id}", CreateBody(user));    // (1)

    public Task<HttpResponseMessage> QueryUser(string arg) =>
        _httpClient.GetAsync($"/users/{int.Parse(arg)}");

    // (1) Serializes user as JSON
    // (2) Sets the Content-Type header
    private static StringContent CreateBody(object user) =>
        new StringContent(                      // (1)
            JsonSerializer.Serialize(user),
            Encoding.UTF8,
            "application/json");                // (2)
}
