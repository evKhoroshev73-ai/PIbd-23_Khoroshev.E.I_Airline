using System;

namespace AirlineManagement.Entities
{
    public class Airplane
    {
        public int Id { get; private set; }
        public string Model { get; private set; }
        public int Capacity { get; private set; }

        public static Airplane CreateEntity(int id, string model, int capacity)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("Модель самолета не может быть пустой или null.", nameof(model));
            }

            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Вместимость должна быть больше нуля.");
            }

            return new Airplane
            {
                Id = id,
                Model = model,
                Capacity = capacity
            };
        }
    }
}