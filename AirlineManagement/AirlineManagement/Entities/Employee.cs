using AirlineManagement.Entities.Enums;

namespace AirlineManagement.Entities
{
    public class Employee
    {
        public int Id { get; private set; }
        public string FullName { get; private set; } = string.Empty;
        public EmployeePost Post { get; private set; }

        public static Employee CreateEntity(int id, string fullName, EmployeePost post)
        {
            return new Employee
            {
                Id = id,
                FullName = fullName ?? string.Empty,
                Post = post
            };
        }
    }
}