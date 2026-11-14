using System;

namespace AirlineManagement.Entities
{
    public class Flight
    {
        public int Id { get; private set; }
        public string FlightNumber { get; private set; }
        public string Destination { get; private set; }
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