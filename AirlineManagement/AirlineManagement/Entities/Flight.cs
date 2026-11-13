using System;

namespace AirlineManagement.Entities
{
    public class Flight
    {
        public int Id { get; private set; }
        public string FlightNumber { get; private set; } = string.Empty;
        public string Destination { get; private set; } = string.Empty;
        public TimeSpan DepartureTime { get; private set; }
        public TimeSpan ArrivalTime { get; private set; }

        public static Flight CreateEntity(int id, string flightNumber, string destination, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            return new Flight
            {
                Id = id,
                FlightNumber = flightNumber ?? string.Empty,
                Destination = destination ?? string.Empty,
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime
            };
        }
    }
}