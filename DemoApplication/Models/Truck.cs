using DemoApplication.Repos;
using Ninject;

namespace DemoApplication.Models
{
    public class Truck : Vehicle
    {
        public string WheelBase { get; set; }

        public Truck() : base(null, "Truck") { } // required empty constructor for sqlite
        [Inject]
        public Truck(IRepository repository) : base(repository, "Truck") { }
    }
}