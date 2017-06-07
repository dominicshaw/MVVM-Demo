using DemoApplication.Models;

namespace DemoApplication.ViewModels
{
    public sealed class TruckViewModel : VehicleViewModel
    {
        private string _wheelBase;
        private Truck _vehicle;
        
        protected override Vehicle Vehicle
        {
            get { return _vehicle; }
            set { _vehicle = (Truck) value; }
        }

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

        public TruckViewModel(Truck truck)
        {
            Load(truck);
            WheelBase = truck.WheelBase;
        }

        public override void Commit()
        {
            base.Commit();
            _vehicle.WheelBase = WheelBase;
        }
    }
}