using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineManagement.Entities;

public sealed class FlightExecutionCrewMember
{
    private FlightExecutionCrewMember() { }

    public int FlightExecutionId { get; private set; }
    public int EmployeeId { get; private set; }

    [ForeignKey(nameof(FlightExecutionId))]
    public FlightExecution FlightExecution { get; private set; } = null!;

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; private set; } = null!;

    public static FlightExecutionCrewMember Create(int employeeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(employeeId);
        return new FlightExecutionCrewMember { EmployeeId = employeeId };
    }
}
