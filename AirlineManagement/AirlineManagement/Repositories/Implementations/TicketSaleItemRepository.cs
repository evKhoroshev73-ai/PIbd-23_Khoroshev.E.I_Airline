using AirlineManagement.Data;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirlineManagement.Repositories.Implementations
{
    public class TicketSaleItemRepository : ITicketSaleItemRepository
    {
        private readonly RepositoryOperations<TicketSaleItem> _operations;

        public TicketSaleItemRepository(IDbContextFactory<DataContext> contextFactory, ILogger<TicketSaleItemRepository> logger)
        {
            _operations = new(contextFactory, logger, entity => entity.Id);
        }

        public IEnumerable<TicketSaleItem> ReadTicketSaleItems() => _operations.ReadAll(nameof(ReadTicketSaleItems));

        // Existing interfaces declare a non-null return, but a missing row historically returns null.
        public TicketSaleItem ReadTicketSaleItemById(int id) => _operations.ReadById(nameof(ReadTicketSaleItemById), id)!;

        public void CreateTicketSaleItem(TicketSaleItem ticketSaleItem) => _operations.Create(nameof(CreateTicketSaleItem), ticketSaleItem);

        public void UpdateTicketSaleItem(TicketSaleItem ticketSaleItem) => _operations.Update(nameof(UpdateTicketSaleItem), ticketSaleItem);

        public void DeleteTicketSaleItem(int id) => _operations.Delete(nameof(DeleteTicketSaleItem), id);
    }
}
