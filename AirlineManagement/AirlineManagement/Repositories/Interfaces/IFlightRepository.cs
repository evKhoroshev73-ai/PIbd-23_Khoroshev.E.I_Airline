using System.Collections.Generic;
using AirlineManagement.Entities;

namespace AirlineManagement.Repositories.Interfaces
{
    public interface IFlightRepository
    {
        IEnumerable<Flight> ReadFlights();
        Flight ReadFlightById(int id);
        void CreateFlight(Flight flight);
        void UpdateFlight(Flight flight);
        void DeleteFlight(int id);
    }
}