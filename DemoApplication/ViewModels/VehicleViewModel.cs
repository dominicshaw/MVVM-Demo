using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DemoApplication.Models;
using DemoApplication.MVVM;
using DemoApplication.Properties;
using log4net;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace DemoApplication.ViewModels
{
    public abstract class VehicleViewModel : INotifyPropertyChanged
    {
        private readonly ILog _log;

        protected abstract Vehicle Vehicle { get; set; }

        private string _type;
        private string _make;
        private string _model;
        private int _capacity;
        private double _price;

        public ICommand SaveVehicleCommand => new AsyncCommand<ObservableCollection<VehicleViewModel>>(SaveVehicle, CanSaveVehicle);
        public ICommand TellMeMoreCommand => new DelegateCommand<VehicleViewModel>(TellMeMore);

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

        public double Price
        {
            get { return _price; }
            set
            {
                if (value == _price) return;
                _price = value;
                OnPropertyChanged();

                PriceHistory[0].Values.Add(new ObservableValue(_price));
            }
        }

        public SeriesCollection PriceHistory { get; } = new SeriesCollection();

        protected VehicleViewModel(ILog log)
        {
            _log = log;
            _log.Info("Creating new VehicleViewModel.");

            PriceHistory.Add(new LineSeries
            {
                Title = "Price (£)",
                Values = new ChartValues<ObservableValue>()
            });
        }

        internal virtual void Load(Vehicle vehicle)
        {
            Vehicle = vehicle;

            Type     = Vehicle.Type;
            Make     = Vehicle.Make;
            Model    = Vehicle.Model;
            Capacity = Vehicle.Capacity;
            Price    = Vehicle.Price;

            PriceHistory[0].Values.Add(new ObservableValue(Price));
        }

        private async Task SaveVehicle(ObservableCollection<VehicleViewModel> vehicles)
        {
            WorkingViewModel.Instance.Working = true;

            _log.Info("Saving VehicleViewModel.");

            Commit();
            await Vehicle.Save();

            if (!vehicles.Contains(this))
                vehicles.Add(this);

            WorkingViewModel.Instance.Working = false;
        }

        private bool CanSaveVehicle(ObservableCollection<VehicleViewModel> vehicles)
        {
            return !string.IsNullOrEmpty(Make) && !string.IsNullOrEmpty(Model);
        }

        public virtual void Commit()
        {
            Vehicle.Type     = Type;
            Vehicle.Make     = Make;
            Vehicle.Model    = Model;
            Vehicle.Capacity = Capacity;
            Vehicle.Price    = Price;
        }
        
        private static void TellMeMore(VehicleViewModel vehicle)
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