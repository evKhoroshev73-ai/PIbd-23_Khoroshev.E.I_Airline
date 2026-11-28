using AirlineManagement.Repositories;
using AirlineManagement.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AirlineManagement.Data;

internal sealed class DataContextFactory(IConnectionString connectionString) : IDbContextFactory<DataContext>
{
    public DataContext CreateDbContext() => new(connectionString);
}

public sealed class DesignTimeDataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args) =>
        new(new ConnectionString(ApplicationConfiguration.Build()));
}
