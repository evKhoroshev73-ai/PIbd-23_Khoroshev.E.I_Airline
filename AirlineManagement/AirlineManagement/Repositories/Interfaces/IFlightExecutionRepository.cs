using System.Collections.Generic;
using AirlineManagement.Entities;

namespace AirlineManagement.Repositories.Interfaces
{
    public interface IFlightExecutionRepository
    {
        IEnumerable<FlightExecution> ReadFlightExecutions();
        FlightExecution ReadFlightExecutionById(int id);
        void CreateFlightExecution(FlightExecution flightExecution);
        void UpdateFlightExecution(FlightExecution flightExecution);
        void DeleteFlightExecution(int id);
    }
}