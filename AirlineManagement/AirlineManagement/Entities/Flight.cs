using System;
using System.ComponentModel.DataAnnotations;

namespace AirlineManagement.Entities
{
    public class Flight
    {
        protected Flight() { }

        [Key]
        public int Id { get; private set; }
        [Required]
        public string FlightNumber { get; private set; } = null!;
        [Required]
        public string Destination { get; private set; } = null!;
        public TimeSpan DepartureTime { get; private set; }
        public TimeSpan ArrivalTime { get; private set; }

        public static Flight CreateEntity(int id, string flightNumber, string destination, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                throw new ArgumentException("Номер рейса не может быть пустым или null.", nameof(flightNumber));
            }

            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException("Пункт назначения не может быть пустым или null.", nameof(destination));
            }

            return new Flight
            {
                Id = id,
                FlightNumber = flightNumber,
                Destination = destination,
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime
            };
        }
    }
}