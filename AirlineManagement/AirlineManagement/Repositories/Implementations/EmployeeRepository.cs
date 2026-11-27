using System.Collections.Generic;
using System.Linq;
using AirlineManagement.Entities;
using AirlineManagement.Repositories.Interfaces;

namespace AirlineManagement.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees = new List<Employee>();

        public IEnumerable<Employee> ReadEmployees() => _employees;

        public Employee ReadEmployeeById(int id) => _employees.FirstOrDefault(e => e.Id == id);

        public void CreateEmployee(Employee employee)
        {
            if (employee != null)
                _employees.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            var existing = ReadEmployeeById(employee.Id);
            if (existing != null)
            {
                _employees.Remove(existing);
                _employees.Add(employee);
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = ReadEmployeeById(id);
            if (employee != null)
                _employees.Remove(employee);
        }
    }
}