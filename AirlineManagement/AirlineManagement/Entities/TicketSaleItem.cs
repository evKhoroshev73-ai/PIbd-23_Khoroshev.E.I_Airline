namespace AirlineManagement.Entities
{
    public class TicketSaleItem
    {
        public int Id { get; private set; }
        public int FlightId { get; private set; }
        public int TicketsSold { get; private set; }
        public decimal PricePerTicket { get; private set; }

        public static TicketSaleItem CreateElement(int id, int flightId, int ticketsSold, decimal pricePerTicket)
        {
            return new TicketSaleItem
            {
                Id = id,
                FlightId = flightId,
                TicketsSold = ticketsSold,
                PricePerTicket = pricePerTicket
            };
        }
    }
}