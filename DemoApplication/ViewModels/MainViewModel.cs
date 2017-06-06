using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DemoApplication.Models;
using DemoApplication.MVVM;
using DemoApplication.Properties;

namespace DemoApplication.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region " background variables "
        private VehicleViewModel _selectedVehicle;
        private readonly BackgroundEmulator _dispatchedWorker;
        private readonly BackgroundEmulator _undispatchedWorker;
        private bool _backgroundThreadUpdates;
        #endregion

        private readonly Repository _repository = new Repository();

        public ObservableCollection<VehicleViewModel> Vehicles { get; } = new ObservableCollection<VehicleViewModel>();

        public ICommand AddVehicleCommand => new DelegateCommand<string>(AddVehicle);

        public VehicleViewModel SelectedVehicle
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
            get { return _backgroundThreadUpdates; }
            set
            {
                if (value == _backgroundThreadUpdates) return;

                _backgroundThreadUpdates = value;

                _dispatchedWorker.BackgroundThreadUpdates = value;
                _undispatchedWorker.BackgroundThreadUpdates = value;

                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _dispatchedWorker   = new DispatchedBackgroundEmulator(Vehicles);
            _undispatchedWorker = new UndispatchedBackgroundEmulator(Vehicles);
        }
        
        public async Task Load()
        {
            try
            {
                WorkingViewModel.Instance.Working = true;

                await _repository.Load();

                foreach (var v in _repository.Vehicles)
                    Vehicles.Add(VehicleFactory.Create(v));

                SelectedVehicle = Vehicles.First();
            }
            finally
            {
                WorkingViewModel.Instance.Working = false;
            }
        }

        private void AddVehicle(string type)
        {
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;

            SelectedVehicle = (VehicleViewModel) Activator.CreateInstance(nameSpace, $"{nameSpace}.ViewModels.{type}").Unwrap();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}