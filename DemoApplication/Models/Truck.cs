using DemoApplication.Repos;

namespace DemoApplication.Models
{
    public class Truck : Vehicle
    {
        public string WheelBase { get; set; }

        public Truck() : base(null, "Truck") { }
        public Truck(LiveRepository repository) : base(repository, "Truck") { }
    }
}