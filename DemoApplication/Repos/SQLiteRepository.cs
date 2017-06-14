using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DemoApplication.Models;
using log4net;
using SQLite;

namespace DemoApplication.Repos
{
    public class SQLiteRepository : IRepository
    {
        private SQLiteAsyncConnection _db;
        private readonly ILog _log;

        public List<Vehicle> Vehicles { get; } = new List<Vehicle>();

        public SQLiteRepository(ILog log)
        {
            _log = log;
            _log.Info("Creating new SQLiteRepository.");
        }

        public async Task Load()
        {
            await CheckAndCreateDatabase();
            await PopulateIfEmpty();

            foreach (var v in await _db.Table<Car>().ToListAsync())
            {
                v.SetRepository(this);
                Vehicles.Add(v);
            }
            foreach (var v in await _db.Table<Truck>().ToListAsync())
            {
                v.SetRepository(this);
                Vehicles.Add(v);
            }

            _log.Info($"Loaded {Vehicles.Count} vehicles from database.");
        }

        private async Task CheckAndCreateDatabase()
        {
            var dir = Path.Combine(Path.GetTempPath(), "Vehicles");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _db = new SQLiteAsyncConnection(Path.Combine(dir, "VehiclesDB.sqlite"));

            await _db.CreateTableAsync<Truck>();
            await _db.CreateTableAsync<Car>();
        }

        private async Task PopulateIfEmpty()
        {
            var counter = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) As Result FROM Car");

            if (counter == 0)
            {
                _log.Info("No database exists; creating database with sample data...");

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

            if (vehicle is Car)
                await _db.InsertAsync(vehicle as Car);
            else if (vehicle is Truck)
                await _db.InsertAsync(vehicle as Truck);
        }
    }
}
