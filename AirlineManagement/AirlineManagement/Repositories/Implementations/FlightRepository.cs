using AirlineManagement.Data;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirlineManagement.Repositories.Implementations
{
    public class FlightRepository : IFlightRepository
    {
        private readonly RepositoryOperations<Flight> _operations;

        public FlightRepository(IDbContextFactory<DataContext> contextFactory, ILogger<FlightRepository> logger)
        {
            _operations = new(contextFactory, logger, entity => entity.Id);
        }

        public IEnumerable<Flight> ReadFlights() => _operations.ReadAll(nameof(ReadFlights));

        // Existing interfaces declare a non-null return, but a missing row historically returns null.
        public Flight ReadFlightById(int id) => _operations.ReadById(nameof(ReadFlightById), id)!;

        public void CreateFlight(Flight flight) => _operations.Create(nameof(CreateFlight), flight);

        public void UpdateFlight(Flight flight) => _operations.Update(nameof(UpdateFlight), flight);

        public void DeleteFlight(int id) => _operations.Delete(nameof(DeleteFlight), id);
    }
}
