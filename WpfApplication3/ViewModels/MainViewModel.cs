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
        #region " background variables "
        private readonly object _sync = new object();
        private readonly Random _random = new Random();
        private readonly Timer _worker;
        private readonly Timer _dispatchedWorker;
        private bool _working;
        private bool _backgroundThreadUpdates;

        private Vehicle _selectedVehicle;
        #endregion

        public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();

        public ICommand SaveVehicleCommand => new AsyncCommand<Vehicle>(SaveVehicle, CanSaveVehicle);
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
        
        public MainViewModel()
        {
            Vehicles.Add(new Car() { Capacity = 5, Make = "Fiat", Model = "Punto", TopSpeed = 70 });
            Vehicles.Add(new Car() { Capacity = 4, Make = "Renault", Model = "Megane", TopSpeed = 80 });
            Vehicles.Add(new Car() { Capacity = 5, Make = "Ford", Model = "Fiesta", TopSpeed = 90});
            Vehicles.Add(new Truck() { Capacity = 3, Make = "Volvo", Model = "BigTruck", WheelBase = "Large" });
            Vehicles.Add(new Truck() { Capacity = 3, Make = "Volvo", Model = "SmallTruck", WheelBase = "Small" });

            _worker = new Timer(o => BackgroundChange(), null, 1000, Timeout.Infinite);
            _dispatchedWorker = new Timer(o => DispatchedChange(), null, 1000, Timeout.Infinite);
        }

        private void DispatchedChange()
        {
            try
            {
                if (!BackgroundThreadUpdates)
                    return;

                var randomVehicle = GetRandomVehicle();

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

        private void BackgroundChange()
        {
            try
            {
                if (!BackgroundThreadUpdates)
                    return;

                var randomVehicle = GetRandomVehicle();

                randomVehicle.Capacity = randomVehicle.Capacity + 1;
            }
            finally
            {
                _worker.Change(7000, Timeout.Infinite);
            }
        }

        private Vehicle GetRandomVehicle()
        {
            lock (_sync)
            {
                return Vehicles[_random.Next(0, Vehicles.Count - 1)];
            }
        }

        private void AddVehicle(string type)
        {
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;

            SelectedVehicle = (Vehicle) Activator.CreateInstance(nameSpace, $"{nameSpace}.ViewModels.{type}").Unwrap();
        }

        private async Task SaveVehicle(Vehicle vehicle)
        {
            Working = true;

            await vehicle.Save();

            if(!Vehicles.Contains(vehicle))
                Vehicles.Add(vehicle);

            Working = false;
        }

        private static bool CanSaveVehicle(Vehicle arg)
        {
            if (arg == null)
                return false;

            return !string.IsNullOrEmpty(arg.Make) && !string.IsNullOrEmpty(arg.Model);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}