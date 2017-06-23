using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DemoApplication.Factories;
using DemoApplication.MVVM;
using DemoApplication.Properties;
using DemoApplication.Repositories;
using log4net;

namespace DemoApplication.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region " background variables "
        private VehicleViewModel _selectedVehicle;
        #endregion

        private readonly ILog _log;
        private readonly IRepository _repository;
        private readonly VehicleViewModelFactory _vehicleFactory;

        public ObservableCollection<VehicleViewModel> Vehicles { get; }

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

        public BackgroundManager BackgroundManager { get; }

        public MainViewModel(ILog log, IRepository repository, VehicleViewModelFactory vehicleFactory, BackgroundManager backgroundManager, ObservableCollection<VehicleViewModel> vehicles)
        {
            _log = log;
            _repository = repository;
            _vehicleFactory = vehicleFactory;

            BackgroundManager = backgroundManager;
            BackgroundManager.PropertyChanged += (s, e) => OnPropertyChanged(nameof(BackgroundManager));

            Vehicles = vehicles;

            _log.Info("MainViewModel initialised.");
        }

        public async Task Load()
        {
            try
            {
                WorkingViewModel.Instance.Working = true;

                await _repository.Initialise();

                foreach (var v in await _repository.GetAll())
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