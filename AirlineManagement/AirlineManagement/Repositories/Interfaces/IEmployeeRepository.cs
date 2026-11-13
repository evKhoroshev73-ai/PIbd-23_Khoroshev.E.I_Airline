using System.Collections.Generic;
using AirlineManagement.Entities;

namespace AirlineManagement.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> ReadEmployees();
        Employee ReadEmployeeById(int id);
        void CreateEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
    }
}