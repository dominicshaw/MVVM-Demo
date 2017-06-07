using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace DemoApplication.Models
{
    public class Repository
    {
        private SQLiteAsyncConnection _db;

        public List<Vehicle> Vehicles { get; } = new List<Vehicle>();
        
        public async Task Load()
        {
            var dir = Path.Combine(Path.GetTempPath(), "Vehicles");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _db = new SQLiteAsyncConnection(Path.Combine(dir, "VehiclesDB.sqlite"));
            
            await _db.CreateTableAsync<Truck>();
            await _db.CreateTableAsync<Car>();

            await PopulateIfEmpty();

            foreach (var c in await _db.Table<Car>().ToListAsync())
                Vehicles.Add(c);
            foreach (var c in await _db.Table<Truck>().ToListAsync())
                Vehicles.Add(c);
        }

        private async Task PopulateIfEmpty()
        {
            if ((await _db.QueryAsync<int>("SELECT COUNT(*) FROM Car")).Count == 0)
            {
                await _db.InsertAsync(new Car   { Capacity = 5, Make = "Fiat"   , Model = "Punto"     , TopSpeed  = 70      });
                await _db.InsertAsync(new Car   { Capacity = 4, Make = "Renault", Model = "Megane"    , TopSpeed  = 80      });
                await _db.InsertAsync(new Car   { Capacity = 5, Make = "Ford"   , Model = "Fiesta"    , TopSpeed  = 90      });
                await _db.InsertAsync(new Truck { Capacity = 3, Make = "Volvo"  , Model = "BigTruck"  , WheelBase = "Large" });
                await _db.InsertAsync(new Truck { Capacity = 3, Make = "Volvo"  , Model = "SmallTruck", WheelBase = "Small" });
            }
        }

        public async Task Save(Vehicle vehicle)
        {
            await _db.InsertAsync(vehicle);

            if (!Vehicles.Contains(vehicle))
                Vehicles.Add(vehicle);
        }
    }
}
