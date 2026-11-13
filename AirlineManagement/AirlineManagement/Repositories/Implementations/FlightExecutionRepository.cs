using System.Collections.Generic;
using System.Linq;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Repositories.Implementations
{
    public class FlightExecutionRepository : IFlightExecutionRepository
    {
        private readonly List<FlightExecution> _executions = new List<FlightExecution>();

        public IEnumerable<FlightExecution> ReadFlightExecutions() => _executions;

        public FlightExecution ReadFlightExecutionById(int id) => _executions.FirstOrDefault(e => e.Id == id);

        public void CreateFlightExecution(FlightExecution flightExecution)
        {
            if (flightExecution != null)
                _executions.Add(flightExecution);
        }

        public void UpdateFlightExecution(FlightExecution flightExecution)
        {
            var existing = ReadFlightExecutionById(flightExecution.Id);
            if (existing != null)
            {
                _executions.Remove(existing);
                _executions.Add(flightExecution);
            }
        }

        public void DeleteFlightExecution(int id)
        {
            var execution = ReadFlightExecutionById(id);
            if (execution != null)
                _executions.Remove(execution);
        }
    }
}