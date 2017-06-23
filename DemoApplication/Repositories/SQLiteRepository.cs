using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DemoApplication.Models;
using log4net;
using SQLite;

namespace DemoApplication.Repositories
{
    public class SQLiteRepository : IRepository
    {
        private SQLiteAsyncConnection _db;
        private readonly ILog _log;
        
        public SQLiteRepository(ILog log)
        {
            _log = log;
            _log.Info("Creating new SQLiteRepository.");
        }

        public async Task Initialise()
        {
            await CheckAndCreateDatabase();
            await PopulateIfEmpty();
        }

        public async Task<List<Vehicle>> GetAll()
        {
            var vehicles = new List<Vehicle>();
            foreach (var v in await _db.Table<Car>().ToListAsync())
            {
                v.SetRepository(this);
                vehicles.Add(v);
            }
            foreach (var v in await _db.Table<Truck>().ToListAsync())
            {
                v.SetRepository(this);
                vehicles.Add(v);
            }

            _log.Info($"Loaded {vehicles.Count} vehicles from database.");
            return vehicles;
        }

        public async Task<List<Car>> GetCarsByMake(string make)
        {
            return await _db.QueryAsync<Car>("SELECT * FROM Car WHERE Make LIKE {0}", make);
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

                await _db.InsertAsync(new Car   { Capacity = 5, Make = "Fiat"   , Model = "Punto"     , TopSpeed  = 70     , Price = 1000  });
                await _db.InsertAsync(new Car   { Capacity = 4, Make = "Renault", Model = "Megane"    , TopSpeed  = 80     , Price = 2000  });
                await _db.InsertAsync(new Car   { Capacity = 5, Make = "Ford"   , Model = "Fiesta"    , TopSpeed  = 90     , Price = 1500  });
                await _db.InsertAsync(new Truck { Capacity = 3, Make = "Volvo"  , Model = "FMX"       , WheelBase = "Large", Price = 10000 });
                await _db.InsertAsync(new Truck { Capacity = 3, Make = "Volvo"  , Model = "VHD"       , WheelBase = "Small", Price = 8000  });
            }
        }

        public async Task Save(Vehicle vehicle)
        {
            await _db.InsertOrReplaceAsync(vehicle);
        }
    }
}
