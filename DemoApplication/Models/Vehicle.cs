namespace DemoApplication.Models
{
    public abstract class Vehicle
    {
        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }

        protected Vehicle(string typ)
        {
            Type = typ;
        }
    }
}
