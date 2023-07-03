namespace AlertService.Services;

/// <summary>
/// Оповещение. Использует appsettings.json.
/// </summary>
public class JsonAlert : IAlert
{
    private readonly IConfiguration _configuration;

    public JsonAlert(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc />
    public string GetMessage()
    {
        return _configuration.GetSection("Alert")["Msg"];
    }
}