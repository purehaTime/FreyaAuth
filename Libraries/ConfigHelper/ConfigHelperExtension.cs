using ConfigHelper.Configs;
using ConfigHelper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigHelper;

public static class ConfigHelperExtension
{
    public static void AddConfigHelper(this IServiceCollection services)
    {
        services.AddScoped<IConfigHelper, Configuration>();
        services.AddOptions<ElasticSettings>().BindConfiguration(nameof(ElasticSettings));
        services.AddOptions<JwtSettings>().BindConfiguration(nameof(JwtSettings));
        services.AddOptions<Service>().BindConfiguration(nameof(Service));
        services.AddOptions<DatabaseSettings>().BindConfiguration(nameof(DatabaseSettings));
        services.AddOptions<EmailSettings>().BindConfiguration(nameof(EmailSettings));
    }
}