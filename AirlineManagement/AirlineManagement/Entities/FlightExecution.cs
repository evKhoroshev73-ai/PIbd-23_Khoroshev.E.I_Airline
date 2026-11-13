using System;
using AirlineManagement.Entities.Enums;

namespace AirlineManagement.Entities
{
    public class FlightExecution
    {
        public int Id { get; private set; }
        public int FlightId { get; private set; }
        public int AirplaneId { get; private set; }
        public DaysOfWeekFlag Days { get; private set; }
        public int OccupiedSeats { get; private set; }
        public DateTime FlightDate { get; private set; }

        public static FlightExecution CreateOperation(int id, int flightId, int airplaneId, DaysOfWeekFlag days, int occupiedSeats, DateTime flightDate)
        {
            return new FlightExecution
            {
                Id = id,
                FlightId = flightId,
                AirplaneId = airplaneId,
                Days = days,
                OccupiedSeats = occupiedSeats,
                FlightDate = flightDate
            };
        }
    }
}