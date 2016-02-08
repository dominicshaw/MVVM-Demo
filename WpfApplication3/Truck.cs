namespace WpfApplication3
{
    public class Truck : Vehicle
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

        public Truck() : base("Truck")
        {

        }
    }
}