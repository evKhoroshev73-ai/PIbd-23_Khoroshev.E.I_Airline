using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirlineManagement.Entities.Enums;

namespace AirlineManagement.Entities;

public class FlightExecution
{
    protected FlightExecution() { }

    [Key]
    public int Id { get; private set; }
    public int FlightId { get; private set; }
    public int AirplaneId { get; private set; }
    public int EmployeeId { get; private set; }
    public DaysOfWeekFlag Days { get; private set; }
    public int OccupiedSeats { get; private set; }

    [Column(TypeName = "date")]
    public DateTime FlightDate { get; private set; }

    public ICollection<FlightExecutionCrewMember> CrewMembers { get; } = new List<FlightExecutionCrewMember>();

    public static FlightExecution CreateOperation(int id, int flightId, int airplaneId, int employeeId,
        DaysOfWeekFlag days, int occupiedSeats, DateTime flightDate, IEnumerable<int>? additionalCrewEmployeeIds = null)
    {
        if (flightId <= 0 || airplaneId <= 0 || employeeId <= 0)
            throw new ArgumentException("Идентификаторы рейса, самолета и сотрудника должны быть больше нуля.");

        ArgumentOutOfRangeException.ThrowIfNegative(occupiedSeats);
        var execution = new FlightExecution
        {
            Id = id,
            FlightId = flightId,
            AirplaneId = airplaneId,
            EmployeeId = employeeId,
            Days = days,
            OccupiedSeats = occupiedSeats,
            FlightDate = flightDate.Date
        };
        foreach (var crewEmployeeId in (additionalCrewEmployeeIds ?? []).Append(employeeId).Distinct())
            execution.CrewMembers.Add(FlightExecutionCrewMember.Create(crewEmployeeId));

        return execution;
    }
}
