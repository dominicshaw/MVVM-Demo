using System.Threading.Tasks;

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

        public override async Task<bool> Save()
        {
            // save base class
            var success = await base.Save();

            if (success)
                await Task.Delay(250); // simulated save of car specific code

            return true;
        }
    }
}