using System.Collections.Generic;
using System.Linq;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Repositories.Implementations
{
    public class TicketSaleItemRepository : ITicketSaleItemRepository
    {
        private readonly List<TicketSaleItem> _items = new List<TicketSaleItem>();

        public IEnumerable<TicketSaleItem> ReadTicketSaleItems() => _items;

        public TicketSaleItem ReadTicketSaleItemById(int id) => _items.FirstOrDefault(i => i.Id == id);

        public void CreateTicketSaleItem(TicketSaleItem ticketSaleItem)
        {
            if (ticketSaleItem != null)
                _items.Add(ticketSaleItem);
        }

        public void UpdateTicketSaleItem(TicketSaleItem ticketSaleItem)
        {
            var existing = ReadTicketSaleItemById(ticketSaleItem.Id);
            if (existing != null)
            {
                _items.Remove(existing);
                _items.Add(ticketSaleItem);
            }
        }

        public void DeleteTicketSaleItem(int id)
        {
            var item = ReadTicketSaleItemById(id);
            if (item != null)
                _items.Remove(item);
        }
    }
}