using System;
using System.Reflection;
using DemoApplication.Models;
using DemoApplication.Repos;
using DemoApplication.ViewModels;
using log4net;

namespace DemoApplication.Factories
{
    public class VehicleViewModelFactory
    {
        private readonly IRepository _repository;
        private readonly ILog _log;

        public VehicleViewModelFactory(IRepository repository, ILog log)
        {
            _repository = repository;
            _log = log;
        }

        public VehicleViewModel Create(Vehicle v)
        {
            if (v is Car)
                return new CarViewModel(_log, v as Car);

            if (v is Truck)
                return new TruckViewModel(_log, v as Truck);

            throw new ArgumentException($"Vehicle is not valid type; \'{v.GetType()}\'.");
        }

        public VehicleViewModel Create(string type)
        {
            var model = Reflect<Vehicle>(type);
            return Create(model);
        }

        private T Reflect<T>(string type)
        {
            // I wouldn't normally use reflection because it is slow; this is just a play with Activator.
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;
            return (T) Activator.CreateInstance(nameSpace, $"{nameSpace}.Models.{type}", false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[] { _repository }, null, null).Unwrap();
        }
    }
}