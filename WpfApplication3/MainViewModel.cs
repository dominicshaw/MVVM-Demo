using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApplication3.Annotations;

namespace WpfApplication3
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Vehicle _selectedVehicle;
        public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();

        public ICommand SaveVehicleCommand => new AsyncCommand<Vehicle>(SaveVehicle, CanSaveVehicle);

        private bool CanSaveVehicle(Vehicle arg)
        {
            return arg?.Make == "Test";
        }

        public ICommand AddTruckCommand => new DelegateCommand(AddTruck);

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

        private readonly Timer _worker;
        private readonly Timer _dispatchedWorker;
        private bool _working;

        public MainViewModel()
        {
            Vehicles.Add(new Car() { Capacity = 5, Make = "Fiat", Model = "Punto" });
            Vehicles.Add(new Car() { Capacity = 4, Make = "Renault", Model = "Megane" });
            Vehicles.Add(new Car() { Capacity = 5, Make = "Ford", Model = "Fiesta" });
            Vehicles.Add(new Truck() { Capacity = 3, Make = "Volvo", Model = "BigTruck", WheelBase = "Large" });
            Vehicles.Add(new Truck() { Capacity = 3, Make = "Volvo", Model = "SmallTruck", WheelBase = "Small" });

            _worker = new Timer(o => ChangeSomething(), null, 1000, Timeout.Infinite);
            _dispatchedWorker = new Timer(o => DispatchedChange(), null, 1000, Timeout.Infinite);
        }

        void DispatchedChange()
        {
            var randomVehicle = Vehicles[new Random().Next(0, Vehicles.Count - 1)];

            Application.Current.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    randomVehicle.Make = randomVehicle.Make + "x";
                }), DispatcherPriority.Normal);

            _dispatchedWorker.Change(6000, Timeout.Infinite);
        }

        void ChangeSomething()
        {
            var randomVehicle = Vehicles[new Random().Next(0, Vehicles.Count - 1)];

            randomVehicle.Capacity = randomVehicle.Capacity + 1;
            _worker.Change(6000, Timeout.Infinite);
        }

        void AddTruck()
        { 
            SelectedVehicle = new Truck();
        }

        async Task SaveVehicle(Vehicle vehicle)
        {
            Working = true;

            // simulated slow database save
            await Task.Delay(500);

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