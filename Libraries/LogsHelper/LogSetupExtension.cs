using ConfigHelper.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace LogsHelper;

public static class LogSetupExtension
{
    public static void UseSerilogHelper(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
        {
            var config = context.Configuration.GetSection(nameof(ElasticSettings)).Get<ElasticSettings>();
            configuration.Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config.Url))
                {
                    IndexFormat = $"{config.Index}-{DateTimeOffset.Now.LocalDateTime:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    OverwriteTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                    TypeName = null,
                    TemplateName = config.TemplateName,
                    BatchAction = ElasticOpType.Create
                });
        });
    }
}