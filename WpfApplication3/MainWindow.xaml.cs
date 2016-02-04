using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using WpfApplication3.Annotations;

namespace WpfApplication3
{
    public abstract class Vehicle : INotifyPropertyChanged
    {
        private string _type;
        private string _make;
        private string _model;
        private int _capacity;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

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

    public class MainViewModel : INotifyPropertyChanged
    {
        private Vehicle _selectedVehicle;
        public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();

        public DelegateCommand AddTruckCommand => new DelegateCommand(AddTruck);

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

        private readonly Timer _worker;
        private readonly Timer _dispatchedWorker;

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
            var truck = new Truck();
            Vehicles.Add(truck);
            SelectedVehicle = truck;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
