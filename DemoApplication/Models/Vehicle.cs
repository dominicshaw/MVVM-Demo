using System;
using System.Threading.Tasks;
using DemoApplication.Repos;
using SQLite;

namespace DemoApplication.Models
{
    public abstract class Vehicle
    {
        private IRepository _repository;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }

        protected Vehicle(IRepository repository, string typ)
        {
            _repository = repository;

            Type = typ;
        }
        
        public async Task Save()
        {
            await _repository.Save(this);
        }

        internal void SetRepository(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}
