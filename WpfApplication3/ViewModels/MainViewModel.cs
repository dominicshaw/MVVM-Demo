using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApplication3.Annotations;
using WpfApplication3.MVVM;

namespace WpfApplication3.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Vehicle _selectedVehicle;
        public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();

        public ICommand SaveVehicleCommand => new AsyncCommand<Vehicle>(SaveVehicle, CanSaveVehicle);

        private bool CanSaveVehicle(Vehicle arg)
        {
            if (arg == null)
                return false;

            return !string.IsNullOrEmpty(arg.Make) && !string.IsNullOrEmpty(arg.Model);
        }

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

        public bool Working
        {
            get { return _working; }
            set
            {
                if (value == _working) return;
                _working = value;
                OnPropertyChanged();
            }
        }

        public bool BackgroundThreadUpdates
        {
            get { return _backgroundThreadUpdates; }
            set
            {
                if (value == _backgroundThreadUpdates) return;
                _backgroundThreadUpdates = value;
                OnPropertyChanged();
            }
        }

        private readonly Timer _worker;
        private readonly Timer _dispatchedWorker;
        private bool _working;
        private bool _backgroundThreadUpdates;

        public MainViewModel()
        {
            Vehicles.Add(new Car() { Capacity = 5, Make = "Fiat", Model = "Punto", TopSpeed = 70 });
            Vehicles.Add(new Car() { Capacity = 4, Make = "Renault", Model = "Megane", TopSpeed = 80 });
            Vehicles.Add(new Car() { Capacity = 5, Make = "Ford", Model = "Fiesta", TopSpeed = 90});
            Vehicles.Add(new Truck() { Capacity = 3, Make = "Volvo", Model = "BigTruck", WheelBase = "Large" });
            Vehicles.Add(new Truck() { Capacity = 3, Make = "Volvo", Model = "SmallTruck", WheelBase = "Small" });

            _worker = new Timer(o => ChangeSomething(), null, 1000, Timeout.Infinite);
            _dispatchedWorker = new Timer(o => DispatchedChange(), null, 1000, Timeout.Infinite);
        }

        void DispatchedChange()
        {
            try
            {
                if (!BackgroundThreadUpdates)
                    return;

                var randomVehicle = Vehicles[new Random().Next(0, Vehicles.Count - 1)];

                Application.Current.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        randomVehicle.Make = randomVehicle.Make + "x";
                    }), DispatcherPriority.Normal);
            }
            finally
            {
                _dispatchedWorker.Change(6000, Timeout.Infinite);
            }
        }

        void ChangeSomething()
        {
            try
            {
                if (!BackgroundThreadUpdates)
                    return;

                var randomVehicle = Vehicles[new Random().Next(0, Vehicles.Count - 1)];

                randomVehicle.Capacity = randomVehicle.Capacity + 1;
            }
            finally
            {
                _worker.Change(6000, Timeout.Infinite);
            }
        }

        void AddVehicle(string type)
        {
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;
            // happens that the assembly name and namespace for the type are the same - this may require amendment if the type moved / assembley renamed
            SelectedVehicle = (Vehicle) Activator.CreateInstance(nameSpace, $"{nameSpace}.{type}").Unwrap();
        }

        async Task SaveVehicle(Vehicle vehicle)
        {
            Working = true;

            await vehicle.Save();

            if(!Vehicles.Contains(vehicle))
                Vehicles.Add(vehicle);

            Working = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}