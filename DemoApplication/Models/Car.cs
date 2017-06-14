using DemoApplication.Repos;
using Ninject;

namespace DemoApplication.Models
{
    public class Car : Vehicle
    {
        public decimal TopSpeed { get; set; }

        public Car() : base(null, "Car") { } // required empty constructor for sqlite
        [Inject]
        public Car(IRepository repository) : base(repository, "Car") { }
    }
}