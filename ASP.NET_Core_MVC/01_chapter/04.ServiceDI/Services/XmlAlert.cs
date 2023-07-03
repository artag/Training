namespace AlertService.Services;

/// <summary>
/// Оповещение. Использует файл settings.xml.
/// </summary>
public class XmlAlert : IAlert
{
    private IConfiguration _configuration;

    public XmlAlert(IWebHostEnvironment env)
    {
        var configurationBuilder = new ConfigurationBuilder();
        // Установка пути, по которому будет осуществляться поиск файла конфигурации.
        configurationBuilder.SetBasePath(env.ContentRootPath);
        // Задание имени файла конфигурации.
        configurationBuilder.AddXmlFile("settings.xml");

        _configuration = configurationBuilder.Build();
    }

    public string GetMessage()
    {
        return _configuration.GetSection("Alert")["Msg"];
    }
}
