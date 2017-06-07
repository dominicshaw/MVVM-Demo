using System;
using System.Reflection;
using DemoApplication.Models;

namespace DemoApplication.ViewModels
{
    public static class VehicleFactory
    {
        public static VehicleViewModel Create(Vehicle v)
        {
            if (v == null)
                throw new ArgumentNullException(nameof(v));

            var car = v as Car;
            if (car != null)
                return new CarViewModel(car);

            var truck = v as Truck;
            if (truck != null)
                return new TruckViewModel(truck);

            throw new ArgumentException($"Vehicle is not valid type; \'{v.GetType()}\'.");
        }

        public static VehicleViewModel Create(string type, Repository repository)
        {
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;
            var model = (Vehicle) Activator.CreateInstance(nameSpace, $"{nameSpace}.Models.{type}", false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[] { repository }, null, null).Unwrap();
            return Create(model);
        }
    }
}