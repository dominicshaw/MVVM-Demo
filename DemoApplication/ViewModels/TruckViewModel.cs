using DemoApplication.Models;
using log4net;

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

        public TruckViewModel(ILog log, Truck truck) : base(log)
        {
            Load(truck);
        }

        internal override void Load(Vehicle vehicle)
        {
            base.Load(vehicle);
            WheelBase = ((Truck) vehicle).WheelBase;
        }

        public override void Commit()
        {
            base.Commit();
            _vehicle.WheelBase = WheelBase;
        }
    }
}