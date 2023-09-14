using ConfigHelper.Interfaces;
using Microsoft.Extensions.Options;

namespace ConfigHelper.Configs;

public class Configuration : IConfigHelper
{
    private readonly IOptions<DatabaseSettings> _dbSettings;

    public Configuration(IOptions<DatabaseSettings> dbSettings)
    {
        _dbSettings = dbSettings;
    }

    public DatabaseSettings DatabaseSettings => _dbSettings.Value;
}