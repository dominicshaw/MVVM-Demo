using System;
using System.Threading.Tasks;
using DemoApplication.Repositories;
using SQLite;

namespace DemoApplication.Models
{
    public abstract class Vehicle
    {
        private IRepository _repository;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } // set by sqlite

        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }

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

        public override bool Equals(object obj) => (obj as Vehicle)?.ID == ID;
        public override int GetHashCode() => ID;
    }
}
