using System;

namespace AirlineManagement.Entities
{
    public class TicketSaleItem
    {
        public int Id { get; private set; }
        public int FlightId { get; private set; }
        public int TicketsSold { get; private set; }
        public decimal PricePerTicket { get; private set; }

        public static TicketSaleItem CreateElement(int id, int flightId, int ticketsSold, decimal pricePerTicket)
        : this()
        {
            if (flightId <= 0)
            {
                throw new ArgumentException("ID рейса должен быть больше нуля.", nameof(flightId));
            }

            if (ticketsSold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ticketsSold), "Количество проданных билетов не может быть отрицательным.");
            }

            if (pricePerTicket < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pricePerTicket), "Цена за билет не может быть отрицательной.");
            }

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