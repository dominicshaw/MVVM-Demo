using DemoApplication.Models;
using log4net;

namespace DemoApplication.ViewModels
{
    public sealed class CarViewModel : VehicleViewModel
    {
        private decimal _topSpeed;
        private Car _vehicle;

        protected override Vehicle Vehicle
        {
            get { return _vehicle; }
            set { _vehicle = (Car)value; }
        }

        public decimal TopSpeed
        {
            get { return _topSpeed; }
            set
            {
                if (value == _topSpeed) return;
                _topSpeed = value;
                OnPropertyChanged();
            }
        }

        public CarViewModel(ILog log, Car car) : base(log)
        {
            Load(car);
        }

        internal override void Load(Vehicle vehicle)
        {
            base.Load(vehicle);
            TopSpeed = ((Car) vehicle).TopSpeed;
        }

        public override void Commit()
        {
            base.Commit();
            _vehicle.TopSpeed = TopSpeed;
        }
    }
}