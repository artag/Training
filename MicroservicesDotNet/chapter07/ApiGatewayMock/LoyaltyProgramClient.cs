using System.Text;
using System.Text.Json;
using Polly;
using Polly.Extensions.Http;

namespace ApiGatewayMock;

public class LoyaltyProgramClient
{
    // (1) Code executed under this policy should return an HttpResponseMessage.
    // (2) Handles all HTTP exceptions
    // (3) Handle timeouts and server errors (5XX and 408 status codes).
    // (4) Chooses an async policy because you'll use it with async code later.
    // (5) Number of retries.
    // (6) Time span to wait before the next retry.
    private static readonly IAsyncPolicy<HttpResponseMessage> ExponentialRetryPolicy =
        Policy<HttpResponseMessage>             // (1)
            .Handle<HttpRequestException>()     // (2)
            .OrTransientHttpStatusCode()        // (3)
            .WaitAndRetryAsync(                 // (4)
                3,                              // (5)
                attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt))  // (6)
            );

    // (1) Handles the same errors as the retry policy.
    // (2) Sets the failure limit to 5 events and
    //     the time-in-open-state limit to 1 minute
    private static readonly IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy =
        Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()                     // (1)
            .OrTransientHttpStatusCode()
            .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));   // (2)

    private readonly HttpClient _httpClient;

    public LoyaltyProgramClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // (1) Executes an action with the retry policy.
    // (2) Sends the command to loyalty program (makes HTTP request).
    public Task<HttpResponseMessage> RegisterUser(string name)
    {
        var user = new { name, Settings = new { } };
        return ExponentialRetryPolicy
            .ExecuteAsync(() =>                                           // (1)
                _httpClient.PostAsync("/users/", CreateBody(user)));      // (2)
    }

    // (1) Sends the UpdateUser command as a PUT request
    public Task<HttpResponseMessage> UpdateUser(LoyaltyProgramUser user) =>
        ExponentialRetryPolicy
            .ExecuteAsync(() =>
                _httpClient.PutAsync($"/users/{user.Id}", CreateBody(user)));  // (1)

    public Task<HttpResponseMessage> QueryUser(string arg) =>
        ExponentialRetryPolicy
            .ExecuteAsync(() =>
                _httpClient.GetAsync($"/users/{int.Parse(arg)}"));

    // (1) Serializes user as JSON
    // (2) Sets the Content-Type header
    private static StringContent CreateBody(object user) =>
        new StringContent(                      // (1)
            JsonSerializer.Serialize(user),
            Encoding.UTF8,
            "application/json");                // (2)
}
