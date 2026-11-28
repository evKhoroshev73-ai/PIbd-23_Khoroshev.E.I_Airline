using AirlineManagement.Data;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirlineManagement.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly RepositoryOperations<Employee> _operations;

        public EmployeeRepository(IDbContextFactory<DataContext> contextFactory, ILogger<EmployeeRepository> logger)
        {
            _operations = new(contextFactory, logger, entity => entity.Id);
        }

        public IEnumerable<Employee> ReadEmployees() => _operations.ReadAll(nameof(ReadEmployees));

        public Employee ReadEmployeeById(int id) => _operations.ReadById(nameof(ReadEmployeeById), id)!;

        public void CreateEmployee(Employee employee) => _operations.Create(nameof(CreateEmployee), employee);

        public void UpdateEmployee(Employee employee) => _operations.Update(nameof(UpdateEmployee), employee);

        public void DeleteEmployee(int id) => _operations.Delete(nameof(DeleteEmployee), id);
    }
}
