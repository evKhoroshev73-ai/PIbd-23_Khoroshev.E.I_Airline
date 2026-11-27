using AirlineManagement.Data;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirlineManagement.Repositories.Implementations
{
    public class AirplaneRepository : IAirplaneRepository
    {
        private readonly RepositoryOperations<Airplane> _operations;

        public AirplaneRepository(IDbContextFactory<DataContext> contextFactory, ILogger<AirplaneRepository> logger)
        {
            _operations = new(contextFactory, logger, entity => entity.Id);
        }

        public IEnumerable<Airplane> ReadAirplanes() => _operations.ReadAll(nameof(ReadAirplanes));

        // Existing interfaces declare a non-null return, but a missing row historically returns null.
        public Airplane ReadAirplaneById(int id) => _operations.ReadById(nameof(ReadAirplaneById), id)!;

        public void CreateAirplane(Airplane airplane) => _operations.Create(nameof(CreateAirplane), airplane);

        public void UpdateAirplane(Airplane airplane) => _operations.Update(nameof(UpdateAirplane), airplane);

        public void DeleteAirplane(int id) => _operations.Delete(nameof(DeleteAirplane), id);
    }
}
