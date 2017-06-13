using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DemoApplication.Models;
using SQLite;

namespace DemoApplication.Repos
{
    public class LiveRepository : IRepository
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

            foreach (var v in await _db.Table<Car>().ToListAsync())
                Vehicles.Add(v);
            foreach (var v in await _db.Table<Truck>().ToListAsync())
                Vehicles.Add(v);
        }

        private async Task PopulateIfEmpty()
        {
            var counter = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) As Result FROM Car");

            if (counter == 0)
            {
                await _db.InsertAsync(new Car   { Capacity = 5, Make = "Fiat"   , Model = "Punto"     , TopSpeed  = 70      });
                await _db.InsertAsync(new Car   { Capacity = 4, Make = "Renault", Model = "Megane"    , TopSpeed  = 80      });
                await _db.InsertAsync(new Car   { Capacity = 5, Make = "Ford"   , Model = "Fiesta"    , TopSpeed  = 90      });
                await _db.InsertAsync(new Truck { Capacity = 3, Make = "Volvo"  , Model = "FMX"       , WheelBase = "Large" });
                await _db.InsertAsync(new Truck { Capacity = 3, Make = "Volvo"  , Model = "VHD"       , WheelBase = "Small" });
            }
        }

        public async Task Save(Vehicle vehicle)
        {
            if (!Vehicles.Contains(vehicle))
                Vehicles.Add(vehicle);

            await _db.InsertAsync(vehicle);
        }
    }
}
