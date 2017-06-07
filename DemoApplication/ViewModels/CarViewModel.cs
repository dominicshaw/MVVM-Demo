using DemoApplication.Models;

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

        public CarViewModel(Car car)
        {
            Load(car);
            TopSpeed = car.TopSpeed;
        }

        public override void Commit()
        {
            base.Commit();
            _vehicle.TopSpeed = TopSpeed;
        }
    }
}