using System;
using System.Reflection;
using DemoApplication.Models;
using DemoApplication.Repos;

namespace DemoApplication.ViewModels
{
    public class VehicleFactory
    {
        private readonly IRepository _repository;

        public VehicleFactory(IRepository repository)
        {
            _repository = repository;
        }

        public VehicleViewModel Create(Vehicle v)
        {
            if (v == null)
                throw new ArgumentNullException(nameof(v));

            var car = v as Car;
            if (car != null)
                return new CarViewModel(car);

            var truck = v as Truck;
            if (truck != null)
                return new TruckViewModel(truck);

            throw new ArgumentException($"Vehicle is not valid type; '{v.GetType()}'.");
        }

        public VehicleViewModel Create(string type)
        {
            // I wouldn't normally use reflection because it is so slow; this is just a play with Activator.
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;
            var model = (Vehicle) Activator.CreateInstance(nameSpace, $"{nameSpace}.Models.{type}", false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[] { _repository }, null, null).Unwrap();
            return Create(model);
        }
    }
}