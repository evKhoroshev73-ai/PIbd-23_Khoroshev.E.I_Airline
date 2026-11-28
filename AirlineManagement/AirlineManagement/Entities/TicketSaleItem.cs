using System;
using System.ComponentModel.DataAnnotations;

namespace AirlineManagement.Entities
{
    public class TicketSaleItem
    {
        protected TicketSaleItem() { }

        [Key]
        public int Id { get; private set; }
        public int FlightId { get; private set; }
        public int EmployeeId { get; private set; }
        public int TicketsSold { get; private set; }
        public decimal PricePerTicket { get; private set; }

        public static TicketSaleItem CreateElement(int id, int flightId, int employeeId, int ticketsSold, decimal pricePerTicket)
        {
            if (flightId <= 0)
            {
                throw new ArgumentException("ID рейса должен быть больше нуля.", nameof(flightId));
            }

            if (employeeId <= 0)
            {
                throw new ArgumentException("ID сотрудника должен быть больше нуля.", nameof(employeeId));
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
                EmployeeId = employeeId,
                TicketsSold = ticketsSold,
                PricePerTicket = pricePerTicket
            };
        }
    }
}