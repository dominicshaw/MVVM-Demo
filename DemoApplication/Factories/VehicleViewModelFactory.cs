using DemoApplication.Models;
using DemoApplication.ViewModels;
using Ninject;
using Ninject.Syntax;

namespace DemoApplication.Factories
{
    public class VehicleViewModelFactory
    {
        private readonly IResolutionRoot _serviceLocator;

        public VehicleViewModelFactory(IResolutionRoot serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public VehicleViewModel Create(Vehicle v)
        {
            var vvm = _serviceLocator.Get<VehicleViewModel>(v.Type);
            vvm.Load(v);

            return vvm;
        }

        public VehicleViewModel Create(string type)
        {
            return _serviceLocator.Get<VehicleViewModel>(type);
        }
    }
}