using System.Collections.Generic;
using AirlineManagement.Entities;

namespace AirlineManagement.Repositories.Interfaces
{
    public interface IAirplaneRepository
    {
        IEnumerable<Airplane> ReadAirplanes();
        Airplane ReadAirplaneById(int id);
        void CreateAirplane(Airplane airplane);
        void UpdateAirplane(Airplane airplane);
        void DeleteAirplane(int id);
    }
}