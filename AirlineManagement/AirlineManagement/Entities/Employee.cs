using System;
using System.ComponentModel.DataAnnotations;
using AirlineManagement.Entities.Enums;

namespace AirlineManagement.Entities
{
    public class Employee
    {
        protected Employee() { }

        [Key]
        public int Id { get; private set; }
        [Required]
        public string FullName { get; private set; } = null!;
        public EmployeePost Post { get; private set; }

        public static Employee CreateEntity(int id, string fullName, EmployeePost post)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("ФИО сотрудника не может быть пустым или null.", nameof(fullName));
            }

            return new Employee
            {
                Id = id,
                FullName = fullName,
                Post = post
            };
        }
    }
}