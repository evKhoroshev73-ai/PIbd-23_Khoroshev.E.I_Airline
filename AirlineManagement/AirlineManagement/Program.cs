using AirlineManagement.Data;
using AirlineManagement.Forms;
using AirlineManagement.Repositories;
using AirlineManagement.Repositories.Implementations;
using AirlineManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Unity;
using Unity.Lifetime;

namespace AirlineManagement;

internal static class Program
{
    public static IUnityContainer Container { get; private set; } = null!;

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var configuration = Data.ApplicationConfiguration.Build();
        using var serilog = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(serilog));
        using var container = BuildContainer(configuration, loggerFactory);
        Container = container;
        Application.Run(container.Resolve<MainForm>());
    }

    internal static IUnityContainer BuildContainer(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var container = new UnityContainer();
        container.RegisterInstance<ILoggerFactory>(loggerFactory, new ExternallyControlledLifetimeManager());
        container.RegisterType(typeof(ILogger<>), typeof(Logger<>));
        container.RegisterInstance<IConnectionString>(new ConnectionString(configuration));
        container.RegisterType<IDbContextFactory<DataContext>, DataContextFactory>(new ContainerControlledLifetimeManager());
        container.RegisterFactory<DataContext>(c => c.Resolve<IDbContextFactory<DataContext>>().CreateDbContext());
        container.RegisterType<IFlightRepository, FlightRepository>();
        container.RegisterType<IAirplaneRepository, AirplaneRepository>();
        container.RegisterType<IEmployeeRepository, EmployeeRepository>();
        container.RegisterType<IFlightExecutionRepository, FlightExecutionRepository>();
        container.RegisterType<ITicketSaleItemRepository, TicketSaleItemRepository>();
        container.RegisterType<MainForm>();
        container.RegisterType<AirplanesForm>();
        container.RegisterType<AirplaneEditForm>();
        container.RegisterType<EmployeesForm>();
        container.RegisterType<EmployeeEditForm>();
        container.RegisterType<FlightsForm>();
        container.RegisterType<FlightEditForm>();
        container.RegisterType<FlightExecutionsForm>();
        container.RegisterType<FlightExecutionAddForm>();
        container.RegisterType<TicketSaleItemsForm>();
        container.RegisterType<TicketSaleItemAddForm>();
        return container;
    }
}
