using Microsoft.Extensions.Configuration;

namespace AirlineManagement.Data;

internal static class ApplicationConfiguration
{
    public static IConfiguration Build() => new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile("appsettings.Local.json", optional: true)
        .AddEnvironmentVariables()
        .Build();
}
