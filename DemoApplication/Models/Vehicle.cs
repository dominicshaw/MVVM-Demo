using System.Threading.Tasks;
using DemoApplication.Repos;

namespace DemoApplication.Models
{
    public abstract class Vehicle
    {
        protected readonly SQLiteRepository _repository;

        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }

        protected Vehicle(SQLiteRepository repository, string typ)
        {
             _repository = repository;

            Type = typ;
        }
        
        public async Task Save()
        {
            await _repository.Save(this);
        }
    }
}
