namespace AirlineManagement.Entities
{
    public class Airplane
    {
        public int Id { get; private set; }
        public string Model { get; private set; } = string.Empty;
        public int Capacity { get; private set; }

        public static Airplane CreateEntity(int id, string model, int capacity)
        {
            return new Airplane
            {
                Id = id,
                Model = model ?? string.Empty,
                Capacity = capacity
            };
        }
    }
}