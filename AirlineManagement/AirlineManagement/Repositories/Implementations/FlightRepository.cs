using System.Collections.Generic;
using System.Linq;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Repositories.Implementations
{
    public class FlightRepository : IFlightRepository
    {
        private readonly List<Flight> _flights = new List<Flight>();

        public IEnumerable<Flight> ReadFlights() => _flights;

        public Flight ReadFlightById(int id) => _flights.FirstOrDefault(f => f.Id == id);

        public void CreateFlight(Flight flight)
        {
            if (flight != null)
                _flights.Add(flight);
        }

        public void UpdateFlight(Flight flight)
        {
            var existing = ReadFlightById(flight.Id);
            if (existing != null)
            {
                _flights.Remove(existing);
                _flights.Add(flight);
            }
        }

        public void DeleteFlight(int id)
        {
            var flight = ReadFlightById(id);
            if (flight != null)
                _flights.Remove(flight);
        }
    }
}