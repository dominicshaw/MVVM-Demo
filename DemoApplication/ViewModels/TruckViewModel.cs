using System.Threading.Tasks;
using DemoApplication.Models;

namespace DemoApplication.ViewModels
{
    public class TruckViewModel : VehicleViewModel
    {
        private string _wheelBase;

        public string WheelBase
        {
            get { return _wheelBase; }
            set
            {
                if (value == _wheelBase) return;
                _wheelBase = value;
                OnPropertyChanged();
            }
        }

        public TruckViewModel(Truck truck) : base(truck)
        {
            WheelBase = truck.WheelBase;
        }

        public override async Task<bool> Save()
        {
            ((Truck) _vehicle).WheelBase = WheelBase;

            // save base class
            var success = await base.Save();

            if (success)
                await Task.Delay(250); // simulated save of truck specific code

            return true;
        }
    }
}