using DemoApplication.Repos;

namespace DemoApplication.Models
{
    public class Car : Vehicle
    {
        public decimal TopSpeed { get; set; }

        public Car() : base(null, "Car") { }
        public Car(LiveRepository repository) : base(repository, "Car") { }
    }
}