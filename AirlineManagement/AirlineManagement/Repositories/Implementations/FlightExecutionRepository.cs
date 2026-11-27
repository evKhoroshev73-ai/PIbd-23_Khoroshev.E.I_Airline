using System.Data;
using AirlineManagement.Data;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirlineManagement.Repositories.Implementations
{
    public class FlightExecutionRepository : IFlightExecutionRepository
    {
        private readonly RepositoryOperations<FlightExecution> _operations;

        public FlightExecutionRepository(IDbContextFactory<DataContext> contextFactory, ILogger<FlightExecutionRepository> logger)
        {
            _operations = new(contextFactory, logger, execution => execution.Id);
        }

        public IEnumerable<FlightExecution> ReadFlightExecutions() =>
            _operations.ReadAll(nameof(ReadFlightExecutions), WithCrew);

        public FlightExecution ReadFlightExecutionById(int id) =>
            _operations.ReadById(nameof(ReadFlightExecutionById), id, WithCrew)!;

        public void CreateFlightExecution(FlightExecution flightExecution)
        {
            const string operation = nameof(CreateFlightExecution);
            _operations.Run(operation, flightExecution?.Id, context =>
            {
                ArgumentNullException.ThrowIfNull(flightExecution);
                _operations.ValidateNew(flightExecution);
                _operations.LogInput(operation, flightExecution);
                var employeeIds = CrewIds(flightExecution);

                using var transaction = context.Database.BeginTransaction();
                // Rebuild the links from their foreign keys; a detached Employee must not be inserted.
                flightExecution.CrewMembers.Clear();
                foreach (var employeeId in employeeIds)
                    flightExecution.CrewMembers.Add(FlightExecutionCrewMember.Create(employeeId));

                context.FlightExecutions.Add(flightExecution);
                var count = context.SaveChanges(); // EF propagates the generated execution ID to the links.
                transaction.Commit();
                _operations.LogWrite(operation, flightExecution.Id, count);
                return count;
            });
        }

        public void UpdateFlightExecution(FlightExecution flightExecution)
        {
            const string operation = nameof(UpdateFlightExecution);
            _operations.Run(operation, flightExecution?.Id, context =>
            {
                ArgumentNullException.ThrowIfNull(flightExecution);
                _operations.LogInput(operation, flightExecution);
                var employeeIds = CrewIds(flightExecution);

                // Prevent concurrent replacements of the same crew from merging into a third list.
                using var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
                var existing = RequireExecution(context, flightExecution.Id);
                context.Entry(existing).CurrentValues.SetValues(flightExecution);

                foreach (var link in existing.CrewMembers.ToArray())
                {
                    // Retain matching links; the remaining IDs require new links.
                    if (!employeeIds.Remove(link.EmployeeId))
                        context.FlightExecutionCrewMembers.Remove(link);
                }

                foreach (var employeeId in employeeIds)
                    existing.CrewMembers.Add(FlightExecutionCrewMember.Create(employeeId));

                var count = context.SaveChanges();
                transaction.Commit();
                _operations.LogWrite(operation, existing.Id, count);
                return count;
            });
        }

        public void DeleteFlightExecution(int id)
        {
            const string operation = nameof(DeleteFlightExecution);
            _operations.Run(operation, id, context =>
            {
                using var transaction = context.Database.BeginTransaction();
                var execution = RequireExecution(context, id);
                context.FlightExecutionCrewMembers.RemoveRange(execution.CrewMembers.ToArray());
                context.FlightExecutions.Remove(execution);
                var count = context.SaveChanges();
                transaction.Commit();
                _operations.LogWrite(operation, id, count);
                return count;
            });
        }

        private static IQueryable<FlightExecution> WithCrew(DataContext context) =>
            context.FlightExecutions.Include(execution => execution.CrewMembers);

        private static FlightExecution RequireExecution(DataContext context, int id) =>
            WithCrew(context).SingleOrDefault(execution => execution.Id == id)
                ?? throw RepositoryOperations<FlightExecution>.Missing(id);

        private static HashSet<int> CrewIds(FlightExecution execution)
        {
            var employeeIds = execution.CrewMembers.Select(link => link.EmployeeId).ToHashSet();
            employeeIds.Add(execution.EmployeeId);
            if (employeeIds.Any(id => id <= 0))
                throw new ArgumentException("Идентификаторы сотрудников экипажа должны быть больше нуля.", nameof(execution));
            return employeeIds;
        }
    }
}
