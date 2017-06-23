using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApplication.Models;

namespace DemoApplication.Repositories
{
    public interface IRepository
    {
        Task Initialise();
        Task<List<Car>> GetCarsByMake(string make);
        Task<List<Vehicle>> GetAll();
        Task Save(Vehicle vehicle);
    }
}