using System.Collections.Generic;
using System.Linq;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Repositories.Implementations
{
    public class AirplaneRepository : IAirplaneRepository
    {
        private readonly List<Airplane> _airplanes = new List<Airplane>();

        public IEnumerable<Airplane> ReadAirplanes() => _airplanes;

        public Airplane ReadAirplaneById(int id) => _airplanes.FirstOrDefault(a => a.Id == id);

        public void CreateAirplane(Airplane airplane)
        {
            if (airplane != null)
                _airplanes.Add(airplane);
        }

        public void UpdateAirplane(Airplane airplane)
        {
            var existing = ReadAirplaneById(airplane.Id);
            if (existing != null)
            {
                _airplanes.Remove(existing);
                _airplanes.Add(airplane);
            }
        }

        public void DeleteAirplane(int id)
        {
            var airplane = ReadAirplaneById(id);
            if (airplane != null)
                _airplanes.Remove(airplane);
        }
    }
}