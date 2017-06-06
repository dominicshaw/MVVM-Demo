using System.Threading.Tasks;
using DemoApplication.Models;

namespace DemoApplication.ViewModels
{
    public class CarViewModel : VehicleViewModel
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
        
        public CarViewModel(Car car) : base(car)
        {
            TopSpeed = car.TopSpeed;
        }

        public override async Task<bool> Save()
        {
            ((Car) _vehicle).TopSpeed = TopSpeed;

            // save base class
            var success = await base.Save();

            if (success)
                await Task.Delay(250); // simulated save of car specific code

            return true;
        }
    }
}