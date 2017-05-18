using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DemoApplication.MVVM;
using DemoApplication.Properties;

namespace DemoApplication.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region " background variables "
        private Vehicle _selectedVehicle;
        private readonly BackgroundEmulator _backgroundWorker;
        #endregion

        public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();

        public ICommand AddVehicleCommand => new DelegateCommand<string>(AddVehicle);

        public Vehicle SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                if (Equals(value, _selectedVehicle)) return;
                _selectedVehicle = value;
                OnPropertyChanged();
            }
        }

        public bool BackgroundThreadUpdates
        {
            get { return _backgroundWorker.BackgroundThreadUpdates; }
            set
            {
                if (value == _backgroundWorker.BackgroundThreadUpdates) return;
                _backgroundWorker.BackgroundThreadUpdates = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Vehicles.Add(new Car() { Capacity = 5, Make = "Fiat", Model = "Punto", TopSpeed = 70 });
            Vehicles.Add(new Car() { Capacity = 4, Make = "Renault", Model = "Megane", TopSpeed = 80 });
            Vehicles.Add(new Car() { Capacity = 5, Make = "Ford", Model = "Fiesta", TopSpeed = 90});
            Vehicles.Add(new Truck() { Capacity = 3, Make = "Volvo", Model = "BigTruck", WheelBase = "Large" });
            Vehicles.Add(new Truck() { Capacity = 3, Make = "Volvo", Model = "SmallTruck", WheelBase = "Small" });

            SelectedVehicle = Vehicles.First();

            _backgroundWorker = new BackgroundEmulator(Vehicles);
        }

        private void AddVehicle(string type)
        {
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;

            SelectedVehicle = (Vehicle) Activator.CreateInstance(nameSpace, $"{nameSpace}.ViewModels.{type}").Unwrap();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}