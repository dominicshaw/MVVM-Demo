using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApplication.Models;

namespace DemoApplication.Repositories
{
    public class TestRepository : IRepository
    {
        private readonly List<Vehicle> _vehicles = new List<Vehicle>();

        public async Task Initialise()
        {
            await Task.Delay(100);

            _vehicles.Add(new Car { Capacity = 5, Make = "Fiat", Model = "Punto", TopSpeed = 70, Price = 1000 });
            _vehicles.Add(new Car { Capacity = 4, Make = "Renault", Model = "Megane", TopSpeed = 80, Price = 2000 });
            _vehicles.Add(new Car { Capacity = 5, Make = "Ford", Model = "Fiesta", TopSpeed = 90, Price = 1500 });
            _vehicles.Add(new Truck { Capacity = 3, Make = "Volvo", Model = "FMX", WheelBase = "Large", Price = 10000 });
            _vehicles.Add(new Truck { Capacity = 3, Make = "Volvo", Model = "VHD", WheelBase = "Small", Price = 8000 });
        }

        public async Task<Vehicle> GetVehicle(int id)
        {
            await Task.Delay(100);
            return _vehicles.Find(v => v.ID == id);
        }

        public async Task<List<Car>> GetCarsByMake(string make)
        {
            await Task.Delay(500);
            return _vehicles.OfType<Car>().Where(x => string.Equals(x.Make, make, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public async Task<List<Vehicle>> GetAll()
        {
            await Task.Delay(1500);
            return _vehicles;
        }

        public async Task Save(Vehicle vehicle)
        {
            await Task.Delay(1000);
        }
    }
}