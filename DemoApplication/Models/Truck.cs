namespace DemoApplication.Models
{
    public class Truck : Vehicle
    {
        public string WheelBase { get; set; }

        public Truck() : base("Truck") { }
    }
}