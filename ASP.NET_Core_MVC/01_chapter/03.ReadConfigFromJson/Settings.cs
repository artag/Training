namespace ReadConfigFromJson;

/// <summary>
/// Настройки приложения.
/// </summary>
public class Settings
{
    public Settings(string rootPath)
    {
        var configurationBuilder = new ConfigurationBuilder();
        // Установка пути, по которому будет выполняться поиск файла конфигурации.
        configurationBuilder.SetBasePath(rootPath);
        // Задание имени файла конфигурации.
        configurationBuilder.AddJsonFile("appsettings.json");
        // Создание конфигурации.
        Config = configurationBuilder.Build();
    }

    /// <summary>
    /// Набор значений параметров сервера.
    /// </summary>
    public IConfiguration Config { get; }
}
