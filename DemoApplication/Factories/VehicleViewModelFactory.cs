using DemoApplication.Models;
using DemoApplication.ViewModels;
using Ninject;

namespace DemoApplication.Factories
{
    public class VehicleViewModelFactory
    {
        private readonly IKernel _kernel;

        public VehicleViewModelFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public VehicleViewModel Create(Vehicle v)
        {
            var vvm = _kernel.Get<VehicleViewModel>(v.Type);
            vvm.Load(v);

            return vvm;
        }

        public VehicleViewModel Create(string type)
        {
            return _kernel.Get<VehicleViewModel>(type);
        }
    }
}