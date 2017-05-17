using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DemoApplication.MVVM;
using DemoApplication.Properties;

namespace DemoApplication.ViewModels
{
    public abstract class Vehicle : INotifyPropertyChanged
    {
        private string _type;
        private string _make;
        private string _model;
        private int _capacity;

        public ICommand SaveVehicleCommand => new AsyncCommand<ObservableCollection<Vehicle>>(SaveVehicle, CanSaveVehicle);
        public ICommand TellMeMoreCommand => new DelegateCommand<Vehicle>(TellMeMore);

        public string Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public string Make
        {
            get { return _make; }
            set
            {
                if (value == _make) return;
                _make = value;
                OnPropertyChanged();
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (value == _model) return;
                _model = value;
                OnPropertyChanged();
            }
        }

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (value == _capacity) return;
                _capacity = value;
                OnPropertyChanged();
            }
        }

        protected Vehicle(string type)
        {
            Type = type;
        }

        private async Task SaveVehicle(ObservableCollection<Vehicle> vehicles)
        {
            WorkingViewModel.Instance.Working = true;

            await Save();

            if (!vehicles.Contains(this))
                vehicles.Add(this);

            WorkingViewModel.Instance.Working = false;
        }

        private bool CanSaveVehicle(ObservableCollection<Vehicle> vehicles)
        {
            return !string.IsNullOrEmpty(Make) && !string.IsNullOrEmpty(Model);
        }

        public virtual async Task<bool> Save()
        {
            // simulated slow database save
            await Task.Delay(250);
            return true;
        }

        private static void TellMeMore(Vehicle vehicle)
        {
            MessageBox.Show(string.Format("Vehicle: {0}", vehicle.GetType()));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}