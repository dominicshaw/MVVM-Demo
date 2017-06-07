using System.Threading.Tasks;

namespace DemoApplication.Models
{
    public abstract class Vehicle
    {
        protected readonly Repository _repository;

        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }

        protected Vehicle(Repository repository, string typ)
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
