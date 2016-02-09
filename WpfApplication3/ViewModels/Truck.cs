using System.Threading.Tasks;

namespace WpfApplication3.ViewModels
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

        public override async Task<bool> Save()
        {
            // save base class
            var success = await base.Save();

            if (success)
                await Task.Delay(250); // simulated save of truck specific code

            return true;
        }
    }
}