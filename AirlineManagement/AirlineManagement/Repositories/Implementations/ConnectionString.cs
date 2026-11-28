using Microsoft.Extensions.Configuration;

namespace AirlineManagement.Repositories.Implementations;

internal sealed class ConnectionString(IConfiguration configuration) : IConnectionString
{
    public string Value { get; } = configuration.GetConnectionString("Airline")
        ?? throw new InvalidOperationException("Не задана строка подключения ConnectionStrings:Airline.");
}
