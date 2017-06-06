namespace DemoApplication.Models
{
    public class Car : Vehicle
    {
        public decimal TopSpeed { get; set; }

        public Car() : base("Car") { }
    }
}