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
            Container.RegisterType<MainForm>();
            Container.RegisterType<AirplanesForm>();
            Container.RegisterType<AirplaneEditForm>();
            Container.RegisterType<EmployeesForm>();
            Container.RegisterType<EmployeeEditForm>();
            Container.RegisterType<FlightsForm>();
            Container.RegisterType<FlightEditForm>();
            Container.RegisterType<FlightExecutionsForm>();
            Container.RegisterType<FlightExecutionAddForm>();
            Container.RegisterType<TicketSaleItemsForm>();
            Container.RegisterType<TicketSaleItemAddForm>();

            Application.Run(Container.Resolve<MainForm>());
        }
    }
}