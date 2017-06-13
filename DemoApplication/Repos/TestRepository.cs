using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApplication.Models;

namespace DemoApplication.Repos
{
    public class TestRepository : IRepository
    {
        public List<Vehicle> Vehicles { get; } = new List<Vehicle>();

        public async Task Load()
        {
            Vehicles.Add(new Car { Capacity = 5, Make = "Fiat", Model = "Punto", TopSpeed = 70 });
            Vehicles.Add(new Car { Capacity = 4, Make = "Renault", Model = "Megane", TopSpeed = 80 });
            Vehicles.Add(new Car { Capacity = 5, Make = "Ford", Model = "Fiesta", TopSpeed = 90 });
            Vehicles.Add(new Truck { Capacity = 3, Make = "Volvo", Model = "FMX", WheelBase = "Large" });
            Vehicles.Add(new Truck { Capacity = 3, Make = "Volvo", Model = "VHD", WheelBase = "Small" });

            await Task.Delay(1500);
        }

        public async Task Save(Vehicle vehicle)
        {
            await Task.Delay(1000);
        }
    }
}