namespace WpfApplication3
{
    public class Car : Vehicle
    {
        private decimal _topSpeed;

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

        public Car() : base("Car")
        {

        }
    }
}