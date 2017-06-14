using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DemoApplication.Emulators;
using DemoApplication.Factories;
using DemoApplication.MVVM;
using DemoApplication.Properties;
using DemoApplication.Repos;
using log4net;

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

        private readonly ILog _log;
        private readonly IRepository _repository;
        private readonly VehicleViewModelFactory _vehicleFactory;

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

        public MainViewModel(ILog log, IRepository repository, VehicleViewModelFactory vehicleFactory)
        {
            _log = log;
            _repository = repository;
            _vehicleFactory = vehicleFactory;

            _dispatchedWorker   = new Dispatched(Vehicles);
            _undispatchedWorker = new Undispatched(Vehicles);

            _log.Info("MainViewModel initialised.");
        }
        
        public async Task Load()
        {
            try
            {
                WorkingViewModel.Instance.Working = true;

                await _repository.Load();

                foreach (var v in _repository.Vehicles)
                    Vehicles.Add(_vehicleFactory.Create(v));

                SelectedVehicle = Vehicles.First();

                _log.Info("MainViewModel loaded.");
            }
            finally
            {
                WorkingViewModel.Instance.Working = false;
            }
        }

        private void AddVehicle(string type)
        {
            SelectedVehicle = _vehicleFactory.Create(type);
        }

        #region " inpc "
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}