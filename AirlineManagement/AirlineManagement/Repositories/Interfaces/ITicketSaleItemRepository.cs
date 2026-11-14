using System.Collections.Generic;
using AirlineManagement.Entities;

namespace AirlineManagement.Repositories.Interfaces
{
    public interface ITicketSaleItemRepository
    {
        IEnumerable<TicketSaleItem> ReadTicketSaleItems();
        void CreateTicketSaleItem(TicketSaleItem item);
    }
}