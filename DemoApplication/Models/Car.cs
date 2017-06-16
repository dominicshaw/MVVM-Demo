using DemoApplication.Repositories;

namespace DemoApplication.Models
{
    public class Car : Vehicle
    {
        public decimal TopSpeed { get; set; }

        public Car() : base(null, "Car") { } // required empty constructor for sqlite
        public Car(IRepository repository) : base(repository, "Car") { }
    }
}