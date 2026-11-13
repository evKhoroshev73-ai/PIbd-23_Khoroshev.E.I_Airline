using System;
using System.Windows.Forms;
using Unity;
using AirlineManagement.Repositories.Interfaces;
using AirlineManagement.Repositories.Implementations;
using AirlineManagement.Forms;

namespace AirlineManagement
{
    static class Program
    {
        public static IUnityContainer Container { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Container = new UnityContainer();

            Container.RegisterType<IFlightRepository, FlightRepository>(new Unity.Lifetime.ContainerControlledLifetimeManager());
            Container.RegisterType<IAirplaneRepository, AirplaneRepository>(new Unity.Lifetime.ContainerControlledLifetimeManager());
            Container.RegisterType<IEmployeeRepository, EmployeeRepository>(new Unity.Lifetime.ContainerControlledLifetimeManager());
            Container.RegisterType<IFlightExecutionRepository, FlightExecutionRepository>(new Unity.Lifetime.ContainerControlledLifetimeManager());
            Container.RegisterType<ITicketSaleItemRepository, TicketSaleItemRepository>(new Unity.Lifetime.ContainerControlledLifetimeManager());

            Application.Run(Container.Resolve<MainForm>());
        }
    }
}