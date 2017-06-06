using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoApplication.Models
{
    public class Repository
    {
        public List<Vehicle> Vehicles { get; } = new List<Vehicle>();
        
        public async Task Load()
        {
            await Task.Delay(1000);

            Vehicles.Add(new Car   { Capacity = 5, Make = "Fiat"   , Model = "Punto"     , TopSpeed  = 70      });
            Vehicles.Add(new Car   { Capacity = 4, Make = "Renault", Model = "Megane"    , TopSpeed  = 80      });
            Vehicles.Add(new Car   { Capacity = 5, Make = "Ford"   , Model = "Fiesta"    , TopSpeed  = 90      });
            Vehicles.Add(new Truck { Capacity = 3, Make = "Volvo"  , Model = "BigTruck"  , WheelBase = "Large" });
            Vehicles.Add(new Truck { Capacity = 3, Make = "Volvo"  , Model = "SmallTruck", WheelBase = "Small" });
        }
    }
}
