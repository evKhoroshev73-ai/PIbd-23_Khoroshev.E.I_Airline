using System.Collections.Generic;
using AirlineManagement.Entities;

namespace AirlineManagement.Repositories.Interfaces
{
    public interface ITicketSaleItemRepository
    {
        IEnumerable<TicketSaleItem> ReadTicketSaleItems();
        TicketSaleItem ReadTicketSaleItemById(int id);
        void CreateTicketSaleItem(TicketSaleItem ticketSaleItem);
        void UpdateTicketSaleItem(TicketSaleItem ticketSaleItem);
        void DeleteTicketSaleItem(int id);
    }
}