using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApplication.Models;

namespace DemoApplication.Repos
{
    public interface IRepository
    {
        List<Vehicle> Vehicles { get; }
        Task Load();
        Task Save(Vehicle vehicle);
    }
}