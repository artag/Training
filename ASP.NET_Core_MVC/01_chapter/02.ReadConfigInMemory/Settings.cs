namespace HelloConfig;

/// <summary>
/// Настройки приложения.
/// </summary>
public class Settings
{
    public Settings()
    {
        var configurationBuilder = new ConfigurationBuilder();

        // Добавление в коллекцию набора значений.
        configurationBuilder.AddInMemoryCollection(
            new Dictionary<string, string>()
            {
                { "Country", "Россия" },
                { "City", "Москва" }
            });

        // Создание конфигурации.
        Config = configurationBuilder.Build();
    }

    /// <summary>
    /// Набор значений параметров сервера.
    /// </summary>
    public IConfiguration Config { get; }
}
